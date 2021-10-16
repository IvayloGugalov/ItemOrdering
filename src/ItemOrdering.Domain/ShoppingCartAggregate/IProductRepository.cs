using System;
using System.Threading.Tasks;

namespace ItemOrdering.Domain.ShoppingCartAggregate
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid productId);
        Task<Product> AddAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
