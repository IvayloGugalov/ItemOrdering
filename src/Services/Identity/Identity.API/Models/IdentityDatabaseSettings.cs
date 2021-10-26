﻿using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Identity.API.Models
{
    public class IdentityDatabaseSettings : IMongoDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string RefreshTokensCollectionName { get; set; }
        public string ConnectionString => $"mongodb://{this.Host}:{this.Port}/identity";
        public string DatabaseName { get; set; }
        public SslSettings SslSettings { get; set; }

        public void ClusterConfigurator(ClusterBuilder obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IMongoDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string RefreshTokensCollectionName { get; set; }
        public string ConnectionString { get; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string DatabaseName { get; set; }
        public SslSettings SslSettings { get; set; }

        void ClusterConfigurator(ClusterBuilder obj);
    }
}
