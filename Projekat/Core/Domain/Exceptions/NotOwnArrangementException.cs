using System;

namespace Domain.Exceptions;

public class NotOwnArrangementException : BadRequestException
{
    public NotOwnArrangementException() : base("You cannot edit or delete someone elses arrangement!")
    {
    }
}
