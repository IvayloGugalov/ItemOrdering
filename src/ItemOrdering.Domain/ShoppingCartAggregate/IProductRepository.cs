using System;
using System.Threading.Tasks;

namespace ItemOrdering.Domain.ShoppingCartAggregate
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(Guid productId);
    }
}
