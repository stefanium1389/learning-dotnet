using System;

namespace Shared.Dtos;

public class AccomodationUnitDto
{
    public Guid Id {get; set;}
    public int MaximumGuests {get; set;}
    public bool PetsAllowed {get; set;}
    public decimal Price {get; set;}
    public bool IsBooked {get; set;}
}
