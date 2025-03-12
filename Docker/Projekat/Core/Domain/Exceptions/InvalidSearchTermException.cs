using System;

namespace Domain.Exceptions;

public class InvalidSearchTermException : BadRequestException
{
    public InvalidSearchTermException(string searchTerm, string searchParam) : base($"Search term '{searchTerm}' is invalid for the field '{searchParam}'")
    {
    }
}
