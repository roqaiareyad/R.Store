using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _idenityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            StoreDbContext context,
            StoreIdentityDbContext idenityDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _idenityDbContext = idenityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            // Create Database If It Doesn't Exist And Apply To Any Pending Migration (Lsa Msma3tesh El DataBase => Update Database)

            if (_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();
            }

            // Data Seeding 

            // Seeding ProductTypes From Json File 

            if (!_context.ProductTypes.Any())
            {
                // 1. Read All Data Types From Types Json File 

                var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                // 2. Transform String To C# Objects (List<ProductTypes>)

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                // 3. Add Data To Database

                if (types is not null && types.Any())
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }


            // Seeding ProductBrands From Json File 

            if (!_context.ProductBrands.Any())
            {
                // 1. Read All Data Brands From Brands Json File 

                var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Data\Persistence\Seeding\brands.json");

                // 2. Transform String To C# Objects (List<ProductBrands>)

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                // 3. Add Data To Database

                if (brandsData is not null && brands.Any())
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }

            }


            // Seeding Products From Json File 

            if (!_context.Products.Any())
            {
                // 1. Read All Data Products From Types Json File 

                var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                // 2. Transform String To C# Objects (List<Products>)

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                // 3. Add Data To Database

                if (products is not null && products.Any())
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }

            }


            // Seeding Delivery From Json File 

            if (!_context.DeliveryMethods.Any())
            {
                // 1. Read All Data Delivery From Types Json File 

                var deliveryData = await File.ReadAllTextAsync(@"..\Infarstructure\Persistence\Data\Seeding\delivery.json");

                // 2. Transform String To C# Objects (List<DeliveryMethod>)

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                // 3. Add Data To Database

                if (deliveryMethods is not null && deliveryMethods.Any())
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _context.SaveChangesAsync();
                }

            }

        }

        public async Task InitializeIdentityAsync()
        {
            // Create DataBase If It doesn't Exist And Apply To Any Pending Migration 
            if (_idenityDbContext.Database.GetPendingMigrations().Any())
            {
                await _idenityDbContext.Database.MigrateAsync();
            }

            if (_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin"

                });

                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"

                });

            }

            // Seeding 

            if (_userManager.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "01234567890"
                };

                var AdminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "01234567890"
                };

                await _userManager.CreateAsync(superAdminUser, "P@ssW0rd");
                await _userManager.CreateAsync(AdminUser, "P@ssW0rd");

                if (await _roleManager.RoleExistsAsync("SuperAdmin"))
                {
                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }

                if (await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _userManager.AddToRoleAsync(superAdminUser, "Admin");
                }
            }


        }


    }
}



// \Infrastructure\Persistence\Seeding\types.json

// \Infrastructure\Persistence\Seeding\brands.json

// \Infrastructure\Persistence\Seeding\products.json