using System;

namespace Domain.Entities;

public class User
{
    public Guid Id {get; set;}
    public string Username {get; set;}
    public string Password {get; set;}
    public string Name {get; set;}
    public string Lastname {get; set;}
    public Gender Gender {get; set;}
    public string Email {get; set;}
    public DateOnly DateOfBirth {get; set;}
    public Role Role {get; set;}
    public IList<Arrangement> CreatedArrangements {get; set;}
    public IList<Reservation> Reservations {get; set;}
}
