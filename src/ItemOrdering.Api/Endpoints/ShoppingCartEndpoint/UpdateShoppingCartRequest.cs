using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public class UpdateShoppingCartRequest
    {
        /// <summary>
        /// Not able to pass {customerId} with [FromRoute] annotation. Passing it inside the PUT call.
        /// </summary>
        public const string ROUTE = "{customerId:guid}/shoppingcart";

        [Required]
        [FromBody]
        public Guid ProductId { get; set; }
    }
}
