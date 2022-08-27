using FluentValidation;
using SampleProject.Framework.Exceptions;
using SampleProject.Application.Exceptions;
using SampleProject.WebApi.Extensions;

namespace SampleProject.WebApi.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e) when (e is ValidationException ex)
            {
                logger.LogInformation(ex, "Validation exception occurred");
                await httpContext.Response.WriteValidationErrorsAsync(ex);
            }
            catch (Exception e) when (e is AppException ex)
            {
                logger.LogInformation(ex, "Application exception occurred");
                await httpContext.Response.WriteApplicationErrorsAsync(ex);
            }
            catch (Exception e) when (e is BusinessException ex)
            {
                logger.LogInformation(ex, "Business exception occurred");
                await httpContext.Response.WriteBusinessErrorsAsync(ex);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Unhandled exception occurred.");
                await httpContext.Response.WriteUnhandledExceptionsAsync();
            }
        }
    }
}
