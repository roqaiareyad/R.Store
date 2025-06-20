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
using Microsoft.Extensions.Configuration;
using Services.Abstractions;
using ServicesAbstractions;
using Shared;
using Stripe;
using OrderProduct = Domain.Models.Product;

namespace Services
{
    public class PaymentsService(IBasketRepository basketRepository,
                                IUnitOfWork unitOfWork,
                                IMapper mapper,
                                IConfiguration configuration) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);

            if (basket is null) throw new BasketNotFoundException(basketId);

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<OrderProduct, int>().GetAsync(item.ProductId);

                if (product is null) throw new ProductNotFoundExceptions(item.ProductId);

                item.Price = product.Price;
            }

            if (!basket.DeliveryMethodId.HasValue) throw new Exception("Invalid Delivery Method Id !!");

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);

            if (deliveryMethod is null) throw new DeliveryMethodNotFoundExceptions(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethod.Cost;

            // Amount 

            var amount = (long)(basket.Items.Sum(I => I.Price * I.Quantity) + basket.ShippingPrice) * 100;

            StripeConfiguration.ApiKey = configuration["StripeSettings:Secretkey"];

            var service = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create 

                var createOptions = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                var paymentIntent = await service.CreateAsync(createOptions);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                // Update 

                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = amount,
                };

                await service.UpdateAsync(basket.PaymentIntentId, updateOptions);

            }

            await basketRepository.UpdateBasketAsync(basket);

            var result = mapper.Map<BasketDto>(basket);

            return result;

        }
    }
}