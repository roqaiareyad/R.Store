using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork , IMapper mapper) : IProductService
    {
        

        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            // Get All Product Through Product Repository
            var Products = await unitOfWork.GetRepository<Product, int>().GetAllAsync();
            // Mapping IEnumerable <Product> To ProductResultDto : AutoMapper
            var result = mapper.Map<IEnumerable<ProductResultDto>>(Products);
            return result;  
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int Id)
        {
          var Product = await unitOfWork.GetRepository<Product , int>().GetAsync(Id);
            if (Product is null) return null;
           var result = mapper.Map<ProductResultDto>(Product);
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
