using System;

namespace Domain.Entities;

public class MeetingPlace
{
    public Guid Id {get; set;}
    public Address Address {get; set;}
    public string Latitude {get; set;}
    public string Longitude {get; set;}
}
