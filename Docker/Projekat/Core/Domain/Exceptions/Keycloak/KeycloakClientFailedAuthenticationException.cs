using System;

namespace Domain.Exceptions.Keycloak;

public class KeycloakClientFailedAuthenticationException  : BadRequestException
{
    public KeycloakClientFailedAuthenticationException() 
        : base("Keycloak Client failed Authentication")
    {
    }

}
