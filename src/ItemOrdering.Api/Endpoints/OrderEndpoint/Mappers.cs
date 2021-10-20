using System.Collections.Generic;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    public static class Mappers
    {
        public static List<OrderDto> MapProductsAndAmountToDTO(this IEnumerable<Order> orders)
        {
            var returnedList = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderedProductsDto = new List<OrderedProductDto>();
                foreach (var (id, price, amount) in order.OrderedProducts)
                {
                    orderedProductsDto.Add(new OrderedProductDto(id, price, amount));
                }

                returnedList.Add(new OrderDto(order.Id, order.ShippingAddress, order.Created, orderedProductsDto));
            }

            return returnedList;
        }
    }
}
