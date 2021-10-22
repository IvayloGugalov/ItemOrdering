using System;
using System.ComponentModel.DataAnnotations;

namespace ItemOrdering.Api.Endpoints.OrderEndpoint
{
    public class GetOrderRequest
    {
        public const string ROUTE = "{customerId:guid}/order/{orderId:guid}";
        public static string BuildRoute(Guid customerId, Guid orderId) =>
            ROUTE
                .Replace("{customerId:guid}", customerId.ToString())
                .Replace("{orderId:guid}", orderId.ToString());

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid OrderId { get; set; }
    }

    public class GetOrdersRequest
    {
        public const string ROUTE = "{customerId:guid}/orders";
        public static string BuildRoute(Guid customerId) => ROUTE.Replace("{customerId:guid}", customerId.ToString());

        [Required]
        public Guid CustomerId { get; set; }
    }
}
