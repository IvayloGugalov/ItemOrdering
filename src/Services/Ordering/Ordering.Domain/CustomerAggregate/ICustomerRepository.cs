using System.Threading.Tasks;

using Ordering.Domain.OrderAggregate;

namespace Ordering.Domain.CustomerAggregate
{
    public interface ICustomerRepository
    {
        Task BuyOrder(Order order);
    }
}
