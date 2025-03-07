using System.Net;

namespace Products.Domain.Exceptions;

/// <summary>
/// Exception for resource not found errors (HTTP 404).
/// </summary>
public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, (int)HttpStatusCode.NotFound) { }
}