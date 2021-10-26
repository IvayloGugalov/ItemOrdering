using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using Ordering.Domain.Test.EntityBuilders;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Test.UnitTests
{
    [TestFixture]
    public class ProductRepositoryTest
    {
        private ItemOrderingDbContext dbContext;
        private ProductRepository productRepository;

        [SetUp]
        public void SetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<ItemOrderingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestOrdering")
                .Options;
            this.dbContext = new ItemOrderingDbContext(dbOptions);
            this.productRepository = new ProductRepository(this.dbContext);
        }

        [Test]
        public async Task GetByIdAsync_WillSucceed()
        {
            var product = ProductBuilder.CreateProduct();

            this.dbContext.SeedDataBaseWith(product);

            var productFromDb = await this.productRepository.GetByIdAsync(product.Id);

            Assert.AreEqual(product, productFromDb);
        }

        [Test]
        public async Task AddAsync_WillSucceed()
        {
            var product = ProductBuilder.CreateProduct();

            await this.productRepository.AddAsync(product);

            var productFromDb = this.dbContext.Products.FirstOrDefault(x => x.Id == product.Id);

            Assert.AreEqual(product, productFromDb);
        }

        [Test]
        public async Task DeleteAsync_WillSucceed()
        {
            var product = ProductBuilder.CreateProduct();

            this.dbContext.Products.Add(product);

            await this.productRepository.DeleteAsync(product);

            var productFromDb = this.dbContext.Products.FirstOrDefault(x => x.Id == product.Id);

            Assert.IsNull(productFromDb);
        }

    }
}
