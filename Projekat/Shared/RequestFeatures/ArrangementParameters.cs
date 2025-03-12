using System;

namespace Shared.RequestFeatures;

public class ArrangementParameters : RequestParameters
{
    public ArrangementParameters() => OrderBy = "StartDate";
    public DateOnly MinStartDate {get; set;} = DateOnly.MinValue;
    public DateOnly MaxStartDate {get; set;} = DateOnly.MaxValue;
    public DateOnly MinEndDate {get; set;} = DateOnly.MinValue;
    public DateOnly MaxEndDate {get; set;} = DateOnly.MaxValue;
    public string ArrangementType {get; set;} = string.Empty;
    public string TransportationType {get; set;} = string.Empty;
    public string Name {get; set;} = string.Empty;
}
