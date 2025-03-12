namespace Shared.Dtos;

public class UserDto
{
    public Guid Id {get; set;}
    public string Username {get; set;}
    public string Name {get; set;}
    public string Lastname {get; set;}
    public string Gender {get; set;}
    public string Email {get; set;}
    public DateOnly DateOfBirth {get; set;}
    public string Role {get; set;}
}
