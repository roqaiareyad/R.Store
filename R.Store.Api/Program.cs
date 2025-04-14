
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstractions;
using AssemblyReference = Services.AssemblyReference;

namespace R.Store.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>( options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
            });
            builder.Services.AddScoped<IDbInitializer,DbInitializer>(); //Allow DI For DbInitializer
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();   
            builder.Services.AddScoped<IServiceManager,ServiceManager>();    
            builder.Services.AddAutoMapper(typeof(AssemblyReference).Assembly);


           var app = builder.Build();

            #region Seeding
            using var Scope = app.Services.CreateScope();
            var dbInitializer = Scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Ask CLR Create Object From DbInitializer
            await dbInitializer.InitializeAsync();
            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
