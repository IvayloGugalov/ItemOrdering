using System;
using System.Threading;
using System.Threading.Tasks;

using Mongo2Go;

using Identity.Infrastructure.MongoDB;
using Identity.Infrastructure.MongoDB.Storages;

namespace Identity.Integration.Tests.Mocks
{
    public class MongoIntegrationTest
    {
        public IMongoDatabaseSettings Options { get; }
        public MongoStorage MongoStorage { get; }

        private readonly MongoDbRunner mongoRunner;

        public MongoIntegrationTest()
        {
            this.mongoRunner = MongoDbRunner.Start();

            this.Options = new IdentityDatabaseSettings
            {
                ConnectionString = this.mongoRunner.ConnectionString,
                DatabaseName = "identity_test"
            };

            this.MongoStorage = new MongoStorage(this.Options);
        }
    }

    public static class TaskExtensions
    {
        public static async Task WithTimeout(this Task task, TimeSpan timeout)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, cancellationTokenSource.Token));
            if (completedTask == task)
            {
                cancellationTokenSource.Cancel();
                await task;
            }
            else
            {
                throw new TimeoutException("The operation has timed out.");
            }
        }
    }
}
