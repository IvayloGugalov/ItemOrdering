using System;
using System.Threading.Tasks;

namespace Ordering.Domain.ShopAggregate
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid productId);
        Task<Product> AddAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
