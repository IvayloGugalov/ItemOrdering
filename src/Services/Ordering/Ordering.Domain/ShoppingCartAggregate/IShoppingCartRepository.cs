using System;
using System.Threading.Tasks;

using Ordering.Domain.Interfaces;

namespace Ordering.Domain.ShoppingCartAggregate
{
    public interface IShoppingCartRepository
    {
        Task AddAsync(ShoppingCart shoppingCart);
        Task UpdateAsync(ShoppingCart shoppingCart);
        Task<ShoppingCart> FindAsync(Guid id);
        Task<ShoppingCart> FindByCustomerAsync(ISpecification<ShoppingCart> specification);
        Task DeleteAsync(ShoppingCart shoppingCart);
    }
}
