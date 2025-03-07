using System.Net;

namespace Products.Domain.Exceptions;

/// <summary>
/// Exception for validation failures (HTTP 400).
/// </summary>
public class ValidationException : AppException
{
    public ValidationException(string message) : base(message, (int)HttpStatusCode.BadRequest) { }
}