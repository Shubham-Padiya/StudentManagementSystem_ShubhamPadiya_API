using FluentValidation;
using StudentManagement.Application.Consts;
using StudentManagement.Application.DTOs;
using System.Net;
using System.Text.Json;

namespace StudentManagement.API.Middlewares
{
    public class GlobalException(RequestDelegate _next,ILogger<GlobalException> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Unexpected);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var response = new ApiResponseDTO<object>();

            switch (exception)
            {
                case ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = ConstantMessages.ValidationError;
                    response.Errors = validationException.Errors
                        .Select(e => new
                        {
                            e.PropertyName,
                            e.ErrorMessage
                        });
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = exception.Message;
                    break;

                case InvalidOperationException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = ConstantMessages.Unauthorize;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = ConstantMessages.Unexpected;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
