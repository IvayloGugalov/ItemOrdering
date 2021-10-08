using System;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    public class CreateOrderResult : CreateOrderCommand
    {
        public Guid Id { get; set; }
    }
}
