using System;
using System.ComponentModel.DataAnnotations;

namespace ItemOrdering.Api.Endpoints.ShoppingCartEndpoint
{
    public class GetShoppingCartRequest
    {
        public const string ROUTE = "{customerId:guid}";
        public static string BuildRoute(Guid customerId) => ROUTE.Replace("{customerId:guid}", customerId.ToString());

        [Required]
        public Guid CustomerId { get; set; }
    }
}
