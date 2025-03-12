using System;

namespace Domain.Entities;

public class AccomodationUnit
{
    public Guid Id {get; set;}
    public int MaximumGuests {get; set;}
    public bool PetsAllowed {get; set;}
    public decimal Price {get; set;}
    public bool IsBooked {get; set;}
    public bool IsDeleted {get; set;}
}
