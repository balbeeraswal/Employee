
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Employees.Filters
{
    public class AsyncExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<AsyncExceptionFilter> _logger;

        public AsyncExceptionFilter(ILogger<AsyncExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception occurred");

            context.Result = new ObjectResult(new
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "An unexpected error occurred.",
                Data = (object?)null
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
