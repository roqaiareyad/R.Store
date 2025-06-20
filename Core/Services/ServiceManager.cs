using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using ServicesAbstractions;
using Shared;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(
            IBasketRepository basketRepository,
            ICacheRepository cacheRepository,
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            IOptions<JwtOptions> options,
            IMapper mapper,
            IConfiguration configuration)
        {
            ProductService = new ProductService(unitOfWork, mapper);
            BasketService = new BasketService(basketRepository, mapper);
            CacheService = new CacheService(cacheRepository);
            AuthService = new AuthService(mapper, userManager, options);
            OrderService = new OrderService(mapper, unitOfWork, basketRepository);
            PaymentService = new PaymentsService(basketRepository, unitOfWork, mapper, configuration);
        }

        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }


        public ICacheService CacheService { get; }
        public IAuthService AuthService { get; }
        public IOrderService OrderService { get; }
        public IPaymentService PaymentService { get; }
    }

}