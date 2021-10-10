using System;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public class ShoppingCartResult
    {
        public Guid Id { get; set; }

        public ShoppingCartResult(Guid id)
        {
            this.Id = id;
        }
    }
}
