using System.Collections.Generic;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    public static class Mappers
    {
        public static List<OrderDto> MapToOrdersDto(this IEnumerable<Order> orders)
        {
            var returnedList = new List<OrderDto>();

            foreach (var order in orders)
            {
                returnedList.Add(order.MapToOrderDto());
            }

            return returnedList;
        }

        public static OrderDto MapToOrderDto(this Order order)
        {
            var orderedProductsDto = new List<OrderedProductDto>();
            foreach (var orderedProduct in order.OrderedProducts)
            {
                orderedProductsDto.Add(new OrderedProductDto(orderedProduct.ProductId, orderedProduct.Price, orderedProduct.Amount));
            }

            return new OrderDto(order.Id, order.ShippingAddress, order.Created, orderedProductsDto);
        }
    }
}
