﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork , IMapper mapper) : IProductService
    {


        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters SpecParam)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(SpecParam);



            // Get All Product Through Product Repository
            var Products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);

            // Mapping IEnumerable <Product> To ProductResultDto : AutoMapper
            var result = mapper.Map<IEnumerable<ProductResultDto>>(Products);

            var specCount = new ProductWithCountSpecification(SpecParam);

            var count = await unitOfWork.GetRepository<Product, int>().CountAsync(specCount);

            return new PaginationResponse<ProductResultDto>(SpecParam.PageIndex, SpecParam.PageSize, count, result);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            //var product = await unitOfWork.GetRepository<Product, int>().GetAsync(id);

            var spec = new ProductWithBrandsAndTypesSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) return null;

            var result = mapper.Map<ProductResultDto>(product);
            return result;
        }
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }


    }
}
