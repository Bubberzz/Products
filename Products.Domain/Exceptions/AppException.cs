using System;

namespace Products.Domain.Exceptions;

/// <summary>
/// Base exception for application-specific errors.
/// </summary>
public abstract class AppException : Exception
{
    public int StatusCode { get; }

    protected AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}