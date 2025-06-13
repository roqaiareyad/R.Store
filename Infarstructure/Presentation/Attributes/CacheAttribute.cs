using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using ServicesAbstractions;

namespace Presentation.Attributes
{
    public class CacheAttribute(int durationInSec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            var result = await CacheService.GetCacheValueAsync(cacheKey);

            if (!string.IsNullOrEmpty(result))
            {
                // Return Responsive
                context.Result = new ContentResult()
                {
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = result
                };

                return;
            }

            // Execute The End Point 
            var contextResult = await next.Invoke();
            if (contextResult.Result is OkObjectResult okObject)
            {
                await CacheService.SetCacheValueAsync(cacheKey, okObject.Value, TimeSpan.FromSeconds(durationInSec));
            }
        }



        private string GenerateCacheKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                key.Append($"|{item.Key}-{item.Value}");
            }

            // /api/Products?brandId=1&typeId=1
            // /api/Products|brandId-1|typeId-1

            return key.ToString();

        }

    }
}
