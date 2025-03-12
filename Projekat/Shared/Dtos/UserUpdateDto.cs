using System;

namespace Shared.Dtos;

public class UserUpdateDto
{
    public string Name {get; set;}
    public string Lastname {get; set;}
    public string Gender {get; set;}
    public string Email {get; set;}
    public DateOnly DateOfBirth {get; set;}
}
