using System;

namespace Domain.Exceptions;

public sealed class DatabaseErrorException : BadRequestException
{
    public DatabaseErrorException(string message) 
        : base(message)
    {
    }
}
