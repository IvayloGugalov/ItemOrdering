using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public class GetShoppingCartRequest
    {
        public const string ROUTE = "{customerId:guid}";

        [Required]
        [FromBody]
        public Guid CustomerId { get; set; }
    }
}
