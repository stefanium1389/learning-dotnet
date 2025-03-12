using System;

namespace Domain.Exceptions.Keycloak;

public class KeycloakUserUpdateException : BadRequestException
{
    public KeycloakUserUpdateException(string username) : base($"User {username} failed to update in Keycloak!")
    {
    }
}
