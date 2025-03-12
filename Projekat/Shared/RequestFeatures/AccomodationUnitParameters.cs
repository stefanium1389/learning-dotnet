using System;

namespace Shared.RequestFeatures;

public class AccomodationUnitParameters : RequestParameters
{
    public int MinGuests { get; set; } = 0;
    public int MaxGuests { get; set; } = int.MaxValue;
    public bool? PetsAllowed {get; set;} = null;
    public bool? IsBooked {get; set;} = null;
    public decimal MinPrice {get; set;} = 0;
    public decimal MaxPrice {get; set;} = int.MaxValue;
}
