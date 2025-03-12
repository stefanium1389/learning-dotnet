using System;

namespace Shared.Dtos;

public class UserCreationDto
{   
    public string Username {get; set;}
    public string Password {get; set;}
    public string Name {get; set;}
    public string Lastname {get; set;}
    public string Gender {get; set;}
    public string Email {get; set;}
    public DateOnly DateOfBirth {get; set;}
}
