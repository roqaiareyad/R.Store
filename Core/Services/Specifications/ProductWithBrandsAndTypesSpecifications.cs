using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;
using static System.Net.WebRequestMethods;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypesSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithBrandsAndTypesSpecifications(int id) : base(p => p.Id == id)
        {
            ApplyInclude();
        }
        public ProductWithBrandsAndTypesSpecifications(ProductSpecificationsParameters SpecParam) : base(
            P =>
            (string.IsNullOrEmpty(SpecParam.Search) || P.Name.ToLower().Contains(SpecParam.Search.ToLower())) &&
            (!SpecParam.BrandId.HasValue || P.BrandId == SpecParam.BrandId) &&
            (!SpecParam.TypeId.HasValue || P.TypeId == SpecParam.TypeId)
            )
        {
            ApplyInclude();
            ApplySorting(SpecParam.Sort);
            ApplyPagination(SpecParam.PageIndex, SpecParam.PageSize);

        }

        private void ApplyInclude()
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {

                    case "namedesc":
                        AddOrderByDescendig(p => p.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescendig(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
        }
    }
}
