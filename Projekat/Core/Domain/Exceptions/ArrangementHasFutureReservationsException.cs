using System;

namespace Domain.Exceptions;

public class ArrangementHasFutureReservationsException : BadRequestException
{
    public ArrangementHasFutureReservationsException() : base("Arrangement has future reservations and cannot be deleted!")
    {
    }
}
