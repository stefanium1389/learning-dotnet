using System;

namespace Domain.Exceptions.Keycloak;

public class KeycloakUserCreationException : BadRequestException

{
    public KeycloakUserCreationException(string username) 
        : base($"User {username} not created in Keycloak!")
    {
    }
}
