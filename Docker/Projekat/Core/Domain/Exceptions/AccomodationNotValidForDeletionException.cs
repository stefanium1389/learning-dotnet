using System;

namespace Domain.Exceptions;

public class AccomodationNotValidForDeletionException : BadRequestException
{
    public AccomodationNotValidForDeletionException(string id) : base($"Accomodation with id: {id} is not valid for deletion!")
    {
    }
}
