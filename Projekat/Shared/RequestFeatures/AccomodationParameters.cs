using System;

namespace Shared.RequestFeatures;

public class AccomodationParameters : RequestParameters
{
    public AccomodationParameters() => OrderBy = "Name";
    public string AccomodationType {get; set;} = string.Empty;
    public string Name {get; set;} = string.Empty;
    public bool? HasPool {get; set;} = null;
    public bool? DisabledFriendly {get; set;} = null;
    public bool? HasSpa {get; set;} = null;
    public bool? HasWifi {get; set;} = null;
}
