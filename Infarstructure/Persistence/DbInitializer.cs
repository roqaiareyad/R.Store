using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbIntializer : IDbInitializer
    {
        private readonly StoreDbContext _context;

        public DbIntializer(StoreDbContext context)
        {
            _context = context;
        }
        public async Task IntializeAsync()
        {
            // Create Database if it doesnt exist && Apply to any pending Migrations
            if (_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();
            }
            // Data Seeding

            // 1) Seeding ProductTypes from json

            if (!_context.ProductTypes.Any())
            {
                // 1.Read Al Data from types json as string
                var TypesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");
                // 2.Transform string to C# Object "List<Product Types>"
                var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                // 3.Add List to DB
                if (types is not null && types.Any())
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }
            // ..\Infrastructure\Persistence\Data\Seeding\types.json

            // 2) Seeding ProductBrands from json

            if (!_context.ProductBrands.Any())
            {
                // 1.Read Al Data from types json as string
                var BrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");
                // 2.Transform string to C# Object "List<Product Types>"
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                // 3.Add List to DB
                if (brands is not null && brands.Any())
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }
            }

            // 3) Seeding Products from json

            if (!_context.Products.Any())
            {
                // 1.Read Al Data from products json as string
                var ProductsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");
                // 2.Transform string to C# Object "List<Product Types>"
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                // 3.Add List to DB
                if (products is not null && products.Any())
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}