using System.Net;

namespace Auth1796.Core.Application.DTOs.Exceptions;

public class InternalServerException : CustomException
{
    public InternalServerException(string message, string errors = default)
        : base(message, errors, HttpStatusCode.InternalServerError)
    {
    }
}