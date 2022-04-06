using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using Identity.Admin;
using Identity.API.Policy;
using Identity.API.Services;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Domain.Services;
using Identity.Infrastructure.MongoDB;
using Identity.Permissions.Interfaces;
using Identity.Tokens;


namespace Identity.API
{
    public class Startup
    {
        private const string ALLOWED_ORIGIN_SETTING = "AllowedMyOriginDomain";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureMongoServices(services);

            this.ConfigureUsedServices(services);

            this.AddAuthentication(services);

            services.AddAdminServices();

            services.AddHttpClient();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity.API", Version = "v1" });
                c.SchemaFilter<CustomSchemaFilters>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API v1"));
                app.UseHttpsRedirection();
                app.UseCors(builder => builder
                    .WithOrigins(this.Configuration[ALLOWED_ORIGIN_SETTING])
                    .SetIsOriginAllowed(host => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            }

            app.UseExceptionHandler(x => x.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var result = JsonSerializer.Serialize(new { error = exception.Message });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{action}");

                endpoints.MapHealthChecks(
                    pattern: "/health/ready",
                    options: new HealthCheckOptions
                    {
                        Predicate = check => check.Tags.Contains("ready"),
                        ResponseWriter = async(context, report) =>
                        {
                            var result = JsonSerializer.Serialize(
                                new
                                {
                                    status = report.Status.ToString(),
                                    checks = report.Entries.Select(entry => new
                                    {
                                        name = entry.Key,
                                        status = entry.Value.Status.ToString(),
                                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                        duration = entry.Value.Duration.ToString()
                                    })
                                });

                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            await context.Response.WriteAsync(result);
                        }
                    });
            });
        }

        private void ConfigureMongoServices(IServiceCollection services)
        {
            services.AddMongoDbStorage()
                .AddTokenServices();

            // Registering the settings here, because the default settings are registered here and not before we create MongoStorage
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonClassMap.RegisterClassMap<AuthUser>(cm =>
            {
                cm.AutoMap();
                cm.MapField("userRoles").SetElementName(nameof(AuthUser.UserRoles));
            });

            services.Configure<IdentityDatabaseSettings>(
                Configuration.GetSection(nameof(IdentityDatabaseSettings)));
            var mongoDbSettings = Configuration.GetSection(nameof(IdentityDatabaseSettings))
                .Get<IdentityDatabaseSettings>();

            services.AddIdentityMongoDbProvider<AuthUser, MongoRole<Guid>, Guid>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequiredUniqueChars = 0;

                    options.User.RequireUniqueEmail = false;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                },
                mongo =>
                {
                    mongo.ConnectionString = mongoDbSettings.ConnectionString;
                });

            services.AddHealthChecks()
                .AddMongoDb(
                    mongoDbSettings.ConnectionString,
                    name: mongoDbSettings.DatabaseName,
                    timeout: TimeSpan.FromSeconds(3),
                    tags: new [] { "ready" });
        }

        private void AddAuthentication(IServiceCollection services)
        {
            var authenticationConfiguration = new AuthenticationConfiguration();
            // Authentication = json object inside appsettings.json
            this.Configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                authenticationConfiguration.CheckJwtConfiguration();

                config.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecretKey)),
                    ValidIssuer = authenticationConfiguration.Issuer,
                    ValidAudience = authenticationConfiguration.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                };
                config.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        private void ConfigureUsedServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionPolicyHandler>();
            services.AddScoped<IUserClaimsPrincipalFactory<AuthUser>, PermissionsToUserClaimsFactory>();

            services.AddTransient<IUsersPermissionsService, UsersPermissionsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IClaimsExtractor, ClaimsExtractor>();
            services.AddTransient<IAuthenticator, Authenticator>();
        }
    }
}
