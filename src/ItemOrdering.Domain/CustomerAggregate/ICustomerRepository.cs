using System.Threading.Tasks;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Domain.CustomerAggregate
{
    public interface ICustomerRepository
    {
        Task BuyOrder(Order order);
    }
}
