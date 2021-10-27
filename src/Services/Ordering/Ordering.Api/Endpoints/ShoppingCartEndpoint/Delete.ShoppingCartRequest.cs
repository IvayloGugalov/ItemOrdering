using System;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    public class DeleteShoppingCartRequest
    {
        public const string ROUTE = "{customerId:guid}/cart";
        public static string BuildRoute(Guid customerId) => ROUTE.Replace("{customerId:guid}", customerId.ToString());

        public Guid CustomerId { get; set; }
    }
}
