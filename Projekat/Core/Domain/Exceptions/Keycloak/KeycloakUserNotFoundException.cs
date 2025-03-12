using System;

namespace Domain.Exceptions.Keycloak;

public class KeycloakUserNotFoundException : NotFoundException
{
    public KeycloakUserNotFoundException(string username) 
        : base($"User {username} not found in Keycloak!")
    {
    }
}
