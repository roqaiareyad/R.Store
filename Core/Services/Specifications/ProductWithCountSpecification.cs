using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithCountSpecification : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecification(ProductSpecificationsParameters SpecParam) : base(
            P =>
            (string.IsNullOrEmpty(SpecParam.Search) || P.Name.ToLower().Contains(SpecParam.Search.ToLower())) &&
            (!SpecParam.BrandId.HasValue || P.BrandId == SpecParam.BrandId) &&
            (!SpecParam.TypeId.HasValue || P.TypeId == SpecParam.TypeId)
            )
        {

        }
    }
}