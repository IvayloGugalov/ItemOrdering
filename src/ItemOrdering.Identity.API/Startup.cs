using System;
using System.Collections.Generic;
using System.Text;

using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using ItemOrdering.Identity.API.Models;
using ItemOrdering.Identity.API.Services.Authenticators;
using ItemOrdering.Identity.API.Services.Repositories;
using ItemOrdering.Identity.API.Services.TokenGenerators;
using ItemOrdering.Identity.API.Services.TokenValidators;


namespace ItemOrdering.Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureMongoServices(services);

            var authenticationConfiguration = new AuthenticationConfiguration();
            // Authentication = json object inside appsettings.json
            this.Configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            this.ConfigureUsedServices(services);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecretKey)),
                        ValidIssuer = authenticationConfiguration.Issuer,
                        ValidAudience = authenticationConfiguration.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ItemOrdering.Identity.API", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ItemOrdering.Identity.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{action}");
            });
        }

        private void ConfigureMongoServices(IServiceCollection services)
        {
            services.Configure<IdentityDatabaseSettings>(
                Configuration.GetSection(nameof(IdentityDatabaseSettings)));

            services.AddSingleton<IMongoDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<IdentityDatabaseSettings>>().Value);

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            services.AddIdentityMongoDbProvider<User, MongoRole<Guid>, Guid>(
                identity =>
                {
                    identity.Password.RequireDigit = false;
                    identity.Password.RequireLowercase = false;
                    identity.Password.RequireNonAlphanumeric = false;
                    identity.Password.RequireUppercase = false;
                    identity.Password.RequiredLength = 1;
                    identity.Password.RequiredUniqueChars = 0;

                    identity.User.RequireUniqueEmail = true;
                },
                mongo =>
                {
                    var databaseSettings = Configuration.GetSection(nameof(IdentityDatabaseSettings))
                        .Get<IdentityDatabaseSettings>();

                    mongo.ConnectionString = databaseSettings.ConnectionString;
                });
        }

        private void ConfigureUsedServices(IServiceCollection services)
        {
            services.AddTransient<IAuthenticator, Authenticator>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IAccessTokenGenerator, AccessTokenGenerator>();
            services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<IRefreshTokenValidator, RefreshTokenValidator>();
        }
    }
}
