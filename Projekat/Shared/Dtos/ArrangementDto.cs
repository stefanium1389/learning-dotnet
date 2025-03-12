using System;

namespace Shared.Dtos;

public class ArrangementDto
{
    public Guid Id {get; set;}
    public string Name { get; set; }
    public string ArrangementType { get; set; }
    public string TransportationType {get; set;}
    public string Destination {get; set;}
    public DateOnly StartDate {get; set;}
    public DateOnly EndDate {get; set;}
    public MeetingPlaceDto MeetingPlace {get; set;}
    public TimeOnly MeetingTime {get; set;}
    public int MaximumPassengers {get; set;}
    public decimal FromPrice {get; set;}
    public string Description {get; set;}
    public string TravelProgramme {get; set;}
    public string PosterUrl {get; set;}
    public AccomodationDto Accomodation {get; set;}
}
