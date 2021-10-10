using System;
using System.ComponentModel.DataAnnotations;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    public class CreateOrderCommand
    {
        public const string ROUTE = "{id}/orders";

        [Required]
        public Guid CustomerId { get; set; }
    }
}
