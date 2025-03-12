using System;
using Domain.Entities;

namespace Domain.Repositories;

public interface IKeycloakRepository
{
    public Task<string> CreateKeycloakUserAsync(User user);
    public Task AssignKeycloakUserRole(User user);
    public Task SetUserEnabled(string username, bool enabled);
    public Task UpdateKeycloakUser(User user);
}
