using System;
using System.Threading.Tasks;

namespace ItemOrdering.Domain.Services
{
    public interface IShoppingCartOrderingService
    {
        Task CreateOrderFromShoppingCart(Guid customerId);
    }
}