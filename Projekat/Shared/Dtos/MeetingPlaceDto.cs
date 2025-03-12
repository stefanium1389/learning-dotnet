using System;

namespace Shared.Dtos;

public class MeetingPlaceDto
{
    public AddressDto Address {get; set;}
    public string Latitude {get; set;}
    public string Longitude {get; set;}
}
