using System;

namespace Domain.Exceptions;

public class ReservationFailedException : BadRequestException
{
    public ReservationFailedException(string message) : base(message)
    {
    }
}
