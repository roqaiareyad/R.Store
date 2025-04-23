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

namespace Presentation.Attributes
{
    public class CacheAttribute(int durationInSecond) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().cacheService;

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            var result = await cacheService.GetCacheValueAsync(cacheKey);
            if (!string.IsNullOrEmpty(result)) // if there is cache
            {
                // Return Response
                context.Result = new ContentResult()
                {
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = result
                };
                return;
            }
            // else Execute endpoint 
            var contextResult = await next.Invoke();
            if (contextResult.Result is OkObjectResult okObject)
            {
                await cacheService.SetCacheValueAsync(cacheKey, okObject.Value, TimeSpan.FromSeconds(durationInSecond));
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
            //api/products|typeid-1|sort-pricedesc
            return key.ToString();
        }
    }
}
