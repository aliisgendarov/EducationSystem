using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EducationSystem.API.ExceptionHandling
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        // SQL Server error numbers
        private const int ForeignKeyViolation = 547;
        private const int UniqueConstraintViolation = 2627;
        private const int UniqueIndexViolation = 2601;

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
            ProblemDetails problem;

            switch (exception)
            {
                case ValidationException validationException:
                    var errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray());

                    problem = new ValidationProblemDetails(errors)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed"
                    };
                    break;

                case SqlException { Number: ForeignKeyViolation }:
                    _logger.LogWarning(exception, "Foreign key violation");
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Related data conflict",
                        Detail = "The record references data that does not exist, " +
                                 "or it is still referenced by other records."
                    };
                    break;

                case SqlException { Number: UniqueConstraintViolation or UniqueIndexViolation }:
                    _logger.LogWarning(exception, "Unique constraint violation");
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Duplicate value",
                        Detail = "A record with the same unique value already exists."
                    };
                    break;

                default:
                    _logger.LogError(exception, "Unhandled exception");
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "An unexpected error occurred"
                    };
                    break;
            }

            httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}