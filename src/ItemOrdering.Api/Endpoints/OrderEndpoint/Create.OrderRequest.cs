﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    public class CreateOrderRequest
    {
        public const string ROUTE = "{customerId}/orders";
        public static string BuildRoute(Guid customerId) => ROUTE.Replace("{customerId}", customerId.ToString());

        [Required]
        public Guid CustomerId { get; set; }
    }
}