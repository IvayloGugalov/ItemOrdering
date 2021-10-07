using System.Threading.Tasks;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Domain.CustomerAggregate
{
    public interface IBuyerRepository
    {
        Task BuyOrder(Order order);
    }
}
