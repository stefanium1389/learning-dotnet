using System;

namespace Shared.Dtos;

public class ReservationDto
{
    public Guid Id {get; set;}
    public UserPreviewDto User {get; set;}
    public bool IsActive {get; set;}
    public ArrangementPreviewDto Arrangement {get; set;}
    public AccomodationUnitDto AccomodationUnit {get; set;}
}
