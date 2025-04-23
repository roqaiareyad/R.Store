using Microsoft.AspNetCore.Mvc;
using Persistence;
using Services;
using Domain.Contracts;
using R.Store.Api.Middlewares;
using Shared.ErrorsModels;


namespace R.Store.Api.Extensions
{
    public static class Extension
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.

            services.AddBuiltinServices();
            services.AddSwaggerServices();



            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices();



            services.ConfigureServices();
            // before build -> allow any service / add any service / allow dependecy injection
            return services;
        }

        private static IServiceCollection AddBuiltinServices(this IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();

            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Add services to the container.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Add services to the container.

            // validation error only
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any()).Select(
                        m => new ValidationError()
                        {
                            Field = m.Key,
                            Errors = m.Value.Errors.Select(error => error.ErrorMessage)
                        }
                        );
                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    }; 
                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }





        public static async Task<WebApplication> ConfigureMiddleWares(this WebApplication app)
        {
            #region Seeding
            app.IntializeDatabaseAsync();
            #endregion

            app.UseGlobalErrorHandling();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();
            return app;
        }


        private static async Task<WebApplication> IntializeDatabaseAsync(this WebApplication app)
        {
            #region Seeding
            using var scope = app.Services.CreateScope();
            var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();// Allow CR Create Object from DbIntializer
            await dbIntializer.IntializeAsync();
            #endregion

            return app;
        }


        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }


    }
}