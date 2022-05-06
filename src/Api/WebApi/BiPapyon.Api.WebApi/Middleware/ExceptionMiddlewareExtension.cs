using BiPapyon.Common.Infrastructure.Exceptions;
using BiPapyon.Common.Models;
using System.Net;

namespace BiPapyon.Api.WebApi.Middleware
{
    public class ExceptionMiddlewareExtension
    {
        private readonly RequestDelegate _next;
        // private readonly ILogger _logger;

        public ExceptionMiddlewareExtension(RequestDelegate next)
        {
            _next = next;
            // _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (PermissionException ex)
            {
                // _logger.LogInformation($"Something went wrong(PermissionException): {ex}");
                await HandleExceptionAsync(httpContext, ex.Message, ex.ErrorCode);
            }
            catch (ServiceException ex)
            {
                //  _logger.LogInformation($"Something went wrong(ServiceException): {ex}");
                await HandleExceptionAsync(httpContext, ex.Message, ex.ErrorCode);
            }
            catch (ValidationException ex)
            {
                // _logger.LogInformation($"Something went wrong(ValidationException): {ex}");

                await HandleExceptionAsync(httpContext, ex.Message, ex.ErrorCode);
            }
            catch (NotFoundException ex)
            {
                // _logger.LogInformation($"Something went wrong(NotFoundException): {ex}");
                await HandleExceptionAsync(httpContext, ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                // _logger.LogInformation($"Something went wrong(Exception): {ex}");

                await HandleExceptionAsync(httpContext, ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, string message, int errorCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            return context.Response.WriteAsync(new ApiResponseDTO()
            {
                StatusCode = errorCode,
                Message = "Bir hata oluştu !",
                Result = null,
                Error = message,
                Success = false
            }.ToString());
        }
    }
}
