using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using ItemOrdering.Domain.OrderAggregate;
using ItemOrdering.Infrastructure.Data;

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
            var customerId = Guid.NewGuid();
            var products = new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 11.24, 3)
            };
            var order = new Order(customerId, products);

            await this.orderRepository.AddAsync(order);

            var orderFromDb = this.dbContext.Orders.FirstOrDefault(x => x.Id == order.Id);

            Assert.AreEqual(order, orderFromDb);
        }
    }
}
