using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemOrdering.Domain.OrderAggregate;
using Microsoft.AspNetCore.Mvc;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    [ApiController]
    public class Get : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public Get(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        //[HttpGet("")]
        //public async Task<ActionResult<int>> 
    }
}
