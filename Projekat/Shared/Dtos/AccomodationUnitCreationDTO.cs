using System;

namespace Shared.Dtos;

public class AccomodationUnitCreationDTO
{
    public int? MaximumGuests {get; set;}
    public bool? PetsAllowed {get; set;}
    public decimal? Price {get; set;}
}
