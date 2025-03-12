using System;

namespace Domain.Exceptions;

public class InvalidEnumValueExcepton : BadRequestException
{
    public InvalidEnumValueExcepton(string value, string field) : base($"Value '{value}' is invalid for the field '{field}'")
    {
    }
}
