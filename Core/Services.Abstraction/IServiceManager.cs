﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesAbstractions;

namespace Services.Abstractions
{
    public interface IServiceManager
    {
         IProductService ProductService { get;}
         IBasketService basketService { get; }
         ICacheService cacheService { get; }
         IAuthService AuthService { get; }

    }
}
