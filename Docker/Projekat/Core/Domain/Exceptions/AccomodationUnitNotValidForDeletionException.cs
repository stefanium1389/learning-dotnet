using System;

namespace Domain.Exceptions;

public class AccomodationUnitNotValidForDeletionException : BadRequestException
{
    public AccomodationUnitNotValidForDeletionException(string id) : base($"Accomodation unit with id: {id} is not valid for deletion!")
    {
    }
}
