using System;

namespace Domain.Exceptions;

public class ManagerNotFoundException : NotFoundException
{
    public ManagerNotFoundException(string username) : base($"Manager with username {username} not found!")
    {
    }
}
