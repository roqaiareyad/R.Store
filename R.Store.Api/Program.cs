
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using R.Store.Api.Extensions;
using R.Store.Api.Middlewares;
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

            builder.Services.RegisterAllServices(builder.Configuration);


            var app = builder.Build();

            // after build configure any middleware

            await app.ConfigureMiddleWares();

            app.Run();
        }
    
    }
}
