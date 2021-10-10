using System;
using System.Threading.Tasks;

namespace ItemOrdering.Domain.ShoppingCartAggregate
{
    public interface IShoppingCartRepository
    {
        Task AddShoppingCart(ShoppingCart shoppingCart);
        Task UpdateShoppingCart(ShoppingCart shoppingCart);
        Task<ShoppingCart> GetShoppingCartForCustomer(Guid customerId);
        Task<ShoppingCart> GetShoppingCartWithProducts(Guid customerId);
    }
}
