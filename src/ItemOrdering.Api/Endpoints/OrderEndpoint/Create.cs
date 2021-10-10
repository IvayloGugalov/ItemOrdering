using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    public class Create : BaseAsyncEndpoint
    {
        private readonly IOrderRepository orderRepository;

        public Create(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpPost(CreateOrderCommand.ROUTE)]
        public async Task<ActionResult<CreateOrderResult>> CreateOrderAsync([FromBody]CreateOrderCommand request)
        {
            var order = new Order(request.CustomerId, new List<OrderedProduct>());

           await this.orderRepository.CreateOrderAsync(order);

            var result = new CreateOrderResult()
            {
                Id = order.Id
            };

            return Ok(result);
        }
    }
}
