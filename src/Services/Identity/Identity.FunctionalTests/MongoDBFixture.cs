using System;
using System.Threading;
using System.Threading.Tasks;

using Mongo2Go;

using Identity.Infrastructure.MongoDB;
using Identity.Infrastructure.MongoDB.Storages;

namespace Identity.FunctionalTests
{
    public class MongoDBFixture : IDisposable
    {
        public IMongoDatabaseSettings Options { get; }
        public MongoStorage MongoStorage { get; }

        private readonly MongoDbRunner mongoRunner;

        private bool isDisposed;

        public MongoDBFixture()
        {
            //this.mongoRunner = MongoDbRunner.Start();

            this.Options = new IdentityDatabaseSettings
            {
                ConnectionString = "mongodb://localhost:49153",
                DatabaseName = "integration"
            };

            this.MongoStorage = new MongoStorage(this.Options);
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.isDisposed = true;
                this.mongoRunner?.Dispose();
            }
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
