using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    public class CreateShoppingCartRequest
    {
        public const string ROUTE = "{CustomerId:guid}/cart";
        public static string BuildRoute(Guid customerId) => ROUTE.Replace("{CustomerId:guid}", customerId.ToString()); 

        [Required]
        public Guid CustomerId { get; set; }
    }
}
