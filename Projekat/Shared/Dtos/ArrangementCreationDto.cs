using System;

namespace Shared.Dtos;

public class ArrangementCreationDto
{
    public string Name { get; set; }
    public string ArrangementType { get; set; }
    public string TransportationType {get; set;}
    public string Destination {get; set;}
    public DateOnly StartDate {get; set;}
    public DateOnly EndDate {get; set;}
    public MeetingPlaceDto MeetingPlace {get; set;}
    public TimeOnly? MeetingTime {get; set;} = null;
    public string Description {get; set;}
    public string TravelProgramme {get; set;}
    public string PosterUrl {get; set;}
    public string AccomodationId {get; set;}
}
