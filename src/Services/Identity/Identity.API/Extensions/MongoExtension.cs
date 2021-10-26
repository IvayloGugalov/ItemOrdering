using GuardClauses;
using MongoDB.Driver;

using Identity.API.Models;

namespace Identity.API.Extensions
{
    public static class MongoExtension
    {
        public static IMongoCollection<TItem> GetCollection<TItem>(IMongoDatabaseSettings options, string collectionName)
        {
            Guard.Against.NullOrEmpty(options.ConnectionString, nameof(options.ConnectionString));

            var type = typeof(TItem);

            var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);

            // TODO: Add ClusterConfigurator and SslSettings
            //settings.SslSettings = options.SslSettings;
            //settings.ClusterConfigurator = options.ClusterConfigurator;

            var client = new MongoClient(settings);
            var collection = client.GetDatabase(options.DatabaseName)
                .GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());

            return collection;
        }
    }
}
