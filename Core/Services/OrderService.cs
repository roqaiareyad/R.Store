using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.OrderModels;
using Services.Abstractions;
using Services.Specifications;
using ServicesAbstractions;
using Shared.OrdersModels;

namespace Services
{
    public class OrderService(IMapper mapper, IUnitOfWork unitOfWork, IBasketRepository basketRepository) : IOrderService
    {
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail)
        {

            // Address
            var address = mapper.Map<Address>(orderRequest.ShipToAddress);

            // Order Item

            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);

            if (basket == null) throw new BasketNotFoundException(orderRequest.BasketId);

            var OrderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.ProductId);

                if (product is null) throw new ProductNotFoundExceptions(item.ProductId);

                var orderItem = new OrderItem(
                    new ProductInOrderItem(product.Id, product.Name, product.PictureUrl),
                    item.Quantity,
                    product.Price
                );

                OrderItems.Add(orderItem);
            }

            // Get Delivery Method 

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundExceptions(orderRequest.DeliveryMethodId);

            // SubTotal 

            var subTotal = OrderItems.Sum(i => i.Price * i.Quantity);

            // TODO :: Create Payment Intent Id 

            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existsOrder = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);

            if (existsOrder is not null)
                unitOfWork.GetRepository<Order, Guid>().Delete(existsOrder);

            // Create Order 

            var order = new Order(userEmail, address, OrderItems, deliveryMethod, subTotal, basket.PaymentIntentId);

            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);

            var count = await unitOfWork.SaveChangesAsync();

            if (count == 0) throw new OrderCreateBadRequestException();

            var result = mapper.Map<OrderResultDto>(order);

            return result;

        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();

            var result = mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);

            return result;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {

            var spec = new OrderSpecifications(id);

            var order = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);

            if (order is null) throw new OrderNotFoundExceptions(id);

            var result = mapper.Map<OrderResultDto>(order);

            return result;

        }

        public async Task<IEnumerable<OrderResultDto>> GetOrdersByEmailAsync(string userEmail)
        {
            var spec = new OrderSpecifications(userEmail);

            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);

            var result = mapper.Map<IEnumerable<OrderResultDto>>(orders);

            return result;
        }
    }
}