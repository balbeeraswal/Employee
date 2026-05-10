using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Employees.Filters
{
    public class CacheResourceFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // Example: check if request is cached
            if (context.HttpContext.Request.Path.Value.Contains("cached"))
            {
                context.Result = new ContentResult
                {
                    Content = "This response is cached",
                    StatusCode = StatusCodes.Status200OK
                };
                return; // short-circuit, skip controller
            }

            // Continue to next stage (model binding, action execution)
            await next();
        }
    }

}
