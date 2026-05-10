
using Employees.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Employees.Filters
{
    public class ApiResponseResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // Only wrap ObjectResult (JSON responses)
            if (context.Result is ObjectResult objectResult)
            {
                var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;

                // If already wrapped in ApiResponse, skip
                if (objectResult.Value is ApiResponse<object>)
                {
                    await next();
                    return;
                }

                // Wrap the response
                objectResult.Value = new ApiResponse<object>
                {
                    StatusCode = statusCode,
                    Message = statusCode == StatusCodes.Status200OK
                        ? "Request processed successfully"
                        : "Request completed with status " + statusCode,
                    Data = objectResult.Value
                };
            }

            await next();
        }
    }

}
