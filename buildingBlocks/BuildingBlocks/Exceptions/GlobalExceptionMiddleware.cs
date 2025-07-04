
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
namespace BuildingBlocks.Exceptions;


public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IMiddleware
{

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // ���������� ������ ������
            await next(context);
        }
        catch (Exception ex)
        {
            // ��������� ����������
            await HandleExceptionAsync(context, ex);
            logger.LogError(ex, "An unexpected error occurred.");

        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // ����������� ��������� �� ������
        var statusCode = HttpStatusCode.InternalServerError;
        var errorType = "UnknownError";
        var errorMessage = exception.Message;

        // ��������� ����������� ����������
        switch (exception)
        {
            case InvalidTokenException:
                statusCode = HttpStatusCode.Unauthorized;
                errorType = nameof(InvalidTokenException);
                errorMessage = exception.Message;
                break;
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorType = nameof(NotFoundException);
                errorMessage = exception.Message;
                break;
            case PasswordIncorrectException:
                statusCode = HttpStatusCode.BadRequest;
                errorType = nameof(PasswordIncorrectException);
                errorMessage = exception.Message;
                break;
            case AlreadyExistsException:
                statusCode = HttpStatusCode.BadRequest;
                errorType = nameof(AlreadyExistsException);
                errorMessage = exception.Message;
                break;
            case ConflictException:
                statusCode = HttpStatusCode.Conflict;
                errorType = nameof(ConflictException);
                errorMessage = exception.Message;
                break;
        }

        // ���������� ������ � ������� JSON
        context.Response.StatusCode = (int)statusCode;
        var result = new
        {
            message = errorMessage,
            type = errorType
        };

        return context.Response.WriteAsJsonAsync(result);
    }
}
