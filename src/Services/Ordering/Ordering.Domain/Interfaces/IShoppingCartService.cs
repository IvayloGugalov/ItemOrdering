using System;
using System.Threading.Tasks;

using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Domain.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetOrCreateShoppingCartAsync(Guid customerId);
        Task<bool> AddProductToShoppingCartAsync(ShoppingCart shoppingCart, Guid productId);
        Task<bool> DeleteAsync(Guid customerId);
    }
}
