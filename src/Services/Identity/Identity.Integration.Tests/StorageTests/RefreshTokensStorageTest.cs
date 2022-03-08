using System;

using NUnit.Framework;

using Identity.Integration.Tests.Mocks;
using Identity.Tokens.Tokens;

namespace Identity.Integration.Tests.StorageTests
{
    [TestFixture]
    public class RefreshTokensStorageTest
    {
        private MongoIntegrationTest mongo;


        [SetUp]
        public void SetUp()
        {
            this.mongo = new MongoIntegrationTest();
        }

        [Test]
        public void A()
        {
            this.mongo.MongoStorage.RefreshTokens.InsertOne(new RefreshToken(Guid.NewGuid().ToString(), Guid.NewGuid()));
        }
    }
}
