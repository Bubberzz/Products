using System.Net;

namespace Products.Domain.Exceptions;

/// <summary>
/// Exception for conflicts (HTTP 409).
/// </summary>
public class ConflictException : AppException
{
    public ConflictException(string message) : base(message, (int)HttpStatusCode.Conflict) { }
}