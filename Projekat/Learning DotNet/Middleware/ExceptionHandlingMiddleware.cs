using System;
using System.Text.Json;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Learning_DotNet.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        var response = new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = "An error occured",
            Type = exception.GetType().Name,
            Detail = exception.Message
        };
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
