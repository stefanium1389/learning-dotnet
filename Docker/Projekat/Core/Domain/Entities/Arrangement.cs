using System;

namespace Domain.Entities;

public class Arrangement
{
    public Guid Id {get; set;}
    public string Name { get; set; }
    public ArrangementType ArrangementType { get; set; }
    public TransportationType TransportationType {get; set;}
    public string Destination {get; set;}
    public DateOnly StartDate {get; set;}
    public DateOnly EndDate {get; set;}
    public MeetingPlace MeetingPlace {get; set;}
    public TimeOnly MeetingTime {get; set;}
    public int MaximumPassengers 
    {
        get => Accomodation?.AccomodationUnits?.Sum(a=>a.MaximumGuests) ?? 0; 
    }
    public decimal FromPrice
    {
        get => Accomodation?.AccomodationUnits?.Min(a => a.Price) ?? 0;
    }
    public string Description {get; set;}
    public string TravelProgramme {get; set;}
    public string PosterUrl {get; set;}
    public Accomodation Accomodation {get; set;}
    public bool IsDeleted {get; set;}
}
