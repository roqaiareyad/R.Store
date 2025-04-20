using Domain.Exceptions;
using Shared.ErrorsModels;

namespace R.Store.Api.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _Logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next ,ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _Logger = logger;
        }



        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Response.ContentType = "application/json";
                    var response = new ErrorDetails
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = $"End Point{context.Request.Path} is Not Found"
                    };
                     await context.Response.WriteAsJsonAsync(response);
                }  
            }
            catch (Exception ex)
            {
                //Log Exception
                _Logger.LogError(ex , ex.Message);

                // 1. Set Status Code For Response
                // 2. Set Content Type Code For Response
                // 3. Response Object Body
                // 4. Return Response

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new ErrorDetails
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };

                response.StatusCode = ex switch 
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,  
                };
                context.Response.StatusCode = response.StatusCode;    



                await context.Response.WriteAsJsonAsync(response);  
            }

        }
    }
}
