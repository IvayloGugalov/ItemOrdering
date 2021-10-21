using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using ItemOrdering.Infrastructure.Data;
using ItemOrdering.Domain.Test.EntityBuilders;

namespace ItemOrdering.Infrastructure.Test.UnitTests
{
    [TestFixture]
    public class OrderRepositoryTest
    {
        private ItemOrderingDbContext dbContext;
        private OrderRepository orderRepository;

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
            var order = OrderBuilder.CreateOrder();

            await this.orderRepository.AddAsync(order);

            var orderFromDb = this.dbContext.Orders.FirstOrDefault(x => x.Id == order.Id);

            Assert.AreEqual(order, orderFromDb);
        }
    }
}
