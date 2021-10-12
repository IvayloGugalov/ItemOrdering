using System;
using System.ComponentModel.DataAnnotations;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public class CreateShoppingCartRequest
    {
        public const string ROUTE = "{id}/cart";

        [Required]
        public Guid CustomerId { get; set; }
    }
}
