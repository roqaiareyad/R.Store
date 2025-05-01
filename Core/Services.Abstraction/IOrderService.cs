using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.OrdersModels;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        // Get Order By Id 

        Task<OrderResultDto> GetOrderByIdAsync(Guid id);

        // Get Orders By Email

        Task<IEnumerable<OrderResultDto>> GetOrdersByEmailAsync(string userEmail);

        // Create Order 

        Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail);

        // Get All Delivery Method

        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods();

    }
}
