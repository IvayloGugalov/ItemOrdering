using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    public class UpdateShoppingCartRequest
    {
        /// <summary>
        /// Not able to pass {customerId} with [FromRoute] annotation. Passing it inside the PUT call.
        /// </summary>
        public const string ROUTE = "{customerId:guid}/cart";
        public static string BuildRoute(Guid customerId) => ROUTE.Replace("{customerId:guid}", customerId.ToString());

        [Required]
        public Guid ProductId { get; set; }
    }
}
