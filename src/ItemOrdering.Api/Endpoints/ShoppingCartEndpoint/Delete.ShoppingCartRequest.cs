using System;

namespace ItemOrdering.Api.Endpoints.ShoppingCartEndpoint
{
    public class DeleteShoppingCartRequest
    {
        public const string ROUTE = "{customerId:guid}/shoppingcart";
        public static string BuildRoute(Guid customerId) => ROUTE.Replace("{customerId:guid}", customerId.ToString());

        public Guid CustomerId { get; set; }
    }
}
