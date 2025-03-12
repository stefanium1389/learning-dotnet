using System;

namespace Shared.RequestFeatures;

public class ReservationParameters : RequestParameters
{
    public bool? Active {get; set;} = null;
    public bool? Future {get; set;} = null;
    public DateOnly MaxStartDate {get; set;} = DateOnly.MaxValue;
    public DateOnly MinStartDate {get; set;} = DateOnly.MinValue;
    public decimal MaxPrice {get; set;} = decimal.MaxValue;
    public decimal MinPrice {get; set;} = 0;
}
