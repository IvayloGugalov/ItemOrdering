using System;
using System.Threading.Tasks;

namespace Ordering.Domain.ShoppingCartAggregate
{
    public interface IShoppingCartRepository
    {
        Task AddAsync(ShoppingCart shoppingCart);
        Task UpdateAsync(ShoppingCart shoppingCart);
        Task<ShoppingCart> FindByCustomerIncludeProducts(Guid customerId);
        Task<ShoppingCart> FindByCustomer(Guid customerId);
        Task DeleteAsync(ShoppingCart shoppingCart);
    }
}
