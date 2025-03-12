using System;

namespace Domain.Exceptions;

public class CommentException : BadRequestException
{
    public CommentException(string message) : base(message)
    {
    }
}
