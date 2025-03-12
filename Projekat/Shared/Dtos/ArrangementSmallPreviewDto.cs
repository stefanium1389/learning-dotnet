using System;

namespace Shared.Dtos;

public class ArrangementSmallPreviewDto
{
    public Guid Id {get; set;}
    public string Name { get; set; }
    public string Destination {get; set;}
    public DateOnly StartDate {get; set;}
}
