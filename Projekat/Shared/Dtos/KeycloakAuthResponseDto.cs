using System;

namespace Shared.Dtos;

public class KeycloakAuthResponseDto
{
    public string access_token {get; set;}
    public int expires_in {get; set;}
    public int refresh_expires_in {get; set;}
    public string token_type {get; set;}
    public int not_before_policy {get; set;}
    public string scope {get; set;}
    public string session_state {get; set;}
    public string refresh_token {get; set;}
}
