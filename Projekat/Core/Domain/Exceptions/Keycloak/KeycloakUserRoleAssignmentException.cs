using System;

namespace Domain.Exceptions.Keycloak;

public class KeycloakUserRoleAssignmentException : BadRequestException
{
    public KeycloakUserRoleAssignmentException(string role, string username) 
        : base($"Keycloak failed to assign role {role} to user {username}!")
    {
    }
}
