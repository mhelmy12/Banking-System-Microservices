using System;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Helpers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

        int statusCode = StatusCodes.Status500InternalServerError;
        ProblemDetails problemDetails;

        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = StatusCodes.Status400BadRequest;

                var errors = validationEx.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );

                problemDetails = new ValidationProblemDetails(errors)
                {
                    Status = statusCode,
                    Title = "Validation Failed",
                    Detail = "There were validation errors in the request.",
                    Instance = httpContext.Request.Path,
                 
                };
                break;

            case DbUpdateException dbUpdateEx when IsUniqueConstraintViolation(dbUpdateEx):
                statusCode = StatusCodes.Status409Conflict;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = "Data Conflict",
                    Detail = "An entity with the same unique value already exists.",
                    Instance = httpContext.Request.Path
                };
                break;

            case InvalidOperationException invalidOpEx:
                statusCode = StatusCodes.Status400BadRequest;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = "Bad Request",
                    Detail = invalidOpEx.Message,
                    Instance = httpContext.Request.Path,

                };
                break;

            default:
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred in the system, please try again later.",
                    Instance = httpContext.Request.Path
                };
                break;
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number is 2601 or 2627;
        }
        return false;
    }

}
