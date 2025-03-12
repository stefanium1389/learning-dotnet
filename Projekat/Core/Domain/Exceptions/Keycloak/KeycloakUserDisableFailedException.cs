using System;

namespace Domain.Exceptions.Keycloak;

public class KeycloakUserDisableFailedException : BadRequestException
{
    public KeycloakUserDisableFailedException(string username) 
        : base($"Keycloak failed to disable user {username}!")
    {
    }
}
