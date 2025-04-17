﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Services.Abstractions
{
    public interface IProductService
    {
        // Get All Product
        Task <PaginationResponse<ProductResultDto>>GetAllProductsAsync(ProductSpecificationsParameters SpecParam);

        // Get Product By Id
        Task<ProductResultDto> GetProductByIdAsync(int id);

        // Get All Types
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        // Get All Brands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
    }
}
