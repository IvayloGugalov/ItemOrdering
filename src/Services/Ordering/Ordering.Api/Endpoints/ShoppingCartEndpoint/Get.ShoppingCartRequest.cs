using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    public class GetShoppingCartRequest
    {
        public const string ROUTE = "{customerId:guid}/cart";
        public static string BuildRoute(Guid customerId) => ROUTE.Replace("{customerId:guid}", customerId.ToString());

        [Required]
        public Guid CustomerId { get; set; }
    }
}
