using System;

namespace Domain.Entities;

public class Reservation
{
    public Guid Id {get; set;}
    public User User {get; set;}
    public bool IsActive {get; set;}
    public Arrangement Arrangement {get; set;}
    public AccomodationUnit AccomodationUnit {get; set;}
    public string Name { get => Arrangement.Name; }
    public decimal Price {get => AccomodationUnit.Price; }
    public DateOnly StartDate {get => Arrangement.StartDate; }

}
