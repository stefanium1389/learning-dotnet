using System;

namespace Shared.Dtos;

public class ArrangementPreviewDto
{
    public Guid Id {get; set;}
    public string Name { get; set; }
    public string ArrangementType { get; set; }
    public string TransportationType {get; set;}
    public string Destination {get; set;}
    public DateOnly StartDate {get; set;}
    public DateOnly EndDate {get; set;}
    public decimal FromPrice {get; set;}
    public string PosterUrl {get; set;}
}
