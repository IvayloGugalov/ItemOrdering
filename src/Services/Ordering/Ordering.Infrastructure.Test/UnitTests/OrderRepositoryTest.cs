using System.Linq;
using System.Threading.Tasks;


using GuidGenerator;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using Ordering.Domain.Test.EntityBuilders;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Test.UnitTests
{
    [TestFixture]
    public class OrderRepositoryTest
    {
        private ItemOrderingDbContext dbContext;
        private OrderRepository orderRepository;
        private readonly IGuidGeneratorService guidGenerator = new GuidGeneratorService();

        [SetUp]
        public void SetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<ItemOrderingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestOrdering")
                .Options;
            this.dbContext = new ItemOrderingDbContext(dbOptions);
            this.orderRepository = new OrderRepository(this.dbContext);
        }

        [Test]
        public async Task CreateAsync_WillSucceed()
        {
            var order = OrderBuilder.CreateOrder(this.guidGenerator);

            await this.orderRepository.AddAsync(order);

            var orderFromDb = this.dbContext.Orders.FirstOrDefault(x => x.Id == order.Id);

            Assert.AreEqual(order, orderFromDb);
        }
    }
}
