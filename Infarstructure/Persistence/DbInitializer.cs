using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;

        public DbInitializer(StoreDbContext context )
        {
           _context = context;
        }  
        public async Task InitializeAsync()
        {
            try
            {
                // Create Database If It doesn't Exists && Apply To Any Pending Migrations

                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }

                // Data Seeding

                //Seeding Product Types From Json File
                if (!_context.ProductTypes.Any())
                {
                    // 1. Read All Data From Types Json File As String
                    var typesData = await File.ReadAllTextAsync(@"..\Infarstructure\Persistence\Data\Seeding\types.json");

                    // 2. Transform from String To C# Object [List<ProductTypes>]
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);


                    // 3. Add List<ProductTypes> To Database
                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }
                }


                //Seeding ProductBrands From Json File
                if (!_context.ProductBrands.Any())
                {
                    // 1. Read All Data From brands Json File As String
                    var brandsData = await File.ReadAllTextAsync(@"..\Infarstructure\Persistence\Data\Seeding\brands.json");

                    // 2. Transform from String To C# Object [List<ProductBrand>]
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);


                    // 3. Add List<ProductTypes> To Database
                    if (brands is not null && brands.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }
                }


                //Seeding Products From Json File
                if (!_context.Products.Any())
                {
                    // 1. Read All Data From products Json File As String
                    var productsData = await File.ReadAllTextAsync(@"..\Infarstructure\Persistence\Data\Seeding\products.json");

                    // 2. Transform from String To C# Object [List<ProductBrand>]
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);


                    // 3. Add List<ProductTypes> To Database
                    if (products is not null && products.Any())
                    {
                        await _context.Products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
//  ..\Infarstructure\Persistence\Data\Seeding\types.json