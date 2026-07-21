using System.Text.Json;
using ProductManagement.Application.Common.Models;
using ProductManagement.Domain.Exceptions;

namespace ProductManagement.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiResponse<object>();

        switch (exception)
        {
            case AppValidationException validationException:

                context.Response.StatusCode = validationException.StatusCode;

                response = ApiResponse<object>.FailureResponse(
                    validationException.Message,
                    validationException.Errors,
                    validationException.StatusCode);

                break;

            case AppException appException:

                context.Response.StatusCode = appException.StatusCode;

                response = ApiResponse<object>.FailureResponse(
                    appException.Message,
                    null,
                    appException.StatusCode);

                break;

            default:

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                response = ApiResponse<object>.FailureResponse(
                    "An unexpected error occurred.",
                    null,
                    StatusCodes.Status500InternalServerError);

                break;
        }

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}