using System;

namespace Shared.Dtos;

public class AccomodationCreationDto
{
    public string AccomodationType { get; set; }
    public string Name { get; set; }
    public int? Stars { get; set; }
    public bool? HasPool { get; set; }
    public bool? HasSpa { get; set; }
    public bool? DisabledFriendly { get; set; }
    public bool? HasWifi { get; set; }
    public IList<AccomodationUnitDto> AccomodationUnits {get; set;}
}
