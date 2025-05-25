using application.Core;
using Microsoft.AspNetCore.Mvc;

namespace asp_services.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path
            };

            // TODO: Log the exception details to a logging service or file
            var innerException = exception.InnerException ?? exception;
            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
            }
            Console.WriteLine($"Url: {context.Request.Path}, Method: {context.Request.Method}, Exception: {innerException.Message}");

            switch (exception)
            {
                case InvalidDataApplicationException _:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Invalid Data";
                    problemDetails.Detail = exception.Message;
                    break;
                case UnauthorizedApplicationException _:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    problemDetails.Detail = exception.Message;
                    break;
                case NotFoundApplicationException _:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Not Found";
                    problemDetails.Detail = exception.Message;
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Detail = "An unexpected error occurred.";
                    break;
            }

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
