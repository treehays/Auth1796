using System.Net;

namespace Auth1796.Core.Application.DTOs.Exceptions;

public class CustomException : Exception
{
    public string ErrorMessages { get; }

    public HttpStatusCode StatusCode { get; }

    public CustomException(string message, string errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorMessages = errors;
        StatusCode = statusCode;
    }
}