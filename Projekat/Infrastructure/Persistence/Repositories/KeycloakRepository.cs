using System;
using System.Net;
using System.Text;
using System.Text.Json;
using Domain.Exceptions.Keycloak;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;

namespace Persistence.Repositories;

public class KeycloakRepository : IKeycloakRepository
{
    public KeycloakRepository(IConfiguration configuration)
    {
        keycloakHost = configuration["Keycloak:AuthServerUrl"]!;
        realm = configuration["Keycloak:Realm"]!;
        client_id = configuration["Keycloak:Resource"]!;
        client_secret = configuration["Keycloak:ResourceClientSecret"]!;
        client_uuid = configuration["Keycloak:ResourceClientUuid"]!;
    }
    private static HttpClient _httpClient = new();
    private readonly string keycloakHost;
    private readonly string realm;
    private readonly string client_id;
    private readonly string client_secret;
    private readonly string client_uuid;
    //Creates a new User in Keycloak and returns his Keycloak user ID.
    public async Task<string> CreateKeycloakUserAsync(User user)
    {
        Uri uri = new($"{keycloakHost}/admin/realms/{realm}/users");
        var keycloakUser = new {
            credentials= new[] {
                new {
                    temporary=false,
                    type = "password",
                    value = user.Password
                }
            },
            firstName = user.Name,
            lastName = user.Lastname,
            email = user.Email,
            emailVerified = true,
            enabled = true,
            username = user.Username
        };
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = uri,
            Headers = {
                {HttpRequestHeader.Authorization.ToString(), $"Bearer {await GetKeycloakClientToken()}"},
            },
            Content = new StringContent(JsonSerializer.Serialize(keycloakUser), Encoding.UTF8, "application/json")
        };
        var response = await _httpClient.SendAsync(httpRequestMessage);
        if( response.StatusCode != HttpStatusCode.Created){
            throw new KeycloakUserCreationException(user.Username);
        }
        return await GetKeycloakUserId(user.Username);
    }
    
    //Gets Keycloak user id for username, used to set id in our database
    private async Task<string> GetKeycloakUserId(string username)
    {
        Uri uri = new($"{keycloakHost}/admin/realms/{realm}/users?username={username}");
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = uri,
            Headers = {
                {HttpRequestHeader.Authorization.ToString(), $"Bearer {await GetKeycloakClientToken()}"},
            },
        };
        var response = await _httpClient.SendAsync(httpRequestMessage);
        if(!response.IsSuccessStatusCode)
        {
            new KeycloakUserNotFoundException(username);
        }
        var obj = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
        return obj![0].GetProperty("id").ToString();
        
    }
    
    //Used to authenticate this client with Keycloak to access the Keycloak Admin REST API
    private async Task<string> GetKeycloakClientToken()
    {
        Uri uri = new($"{keycloakHost}/realms/{realm}/protocol/openid-connect/token");
                var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type","client_credentials"),
            new KeyValuePair<string, string>("client_id",client_id),
            new KeyValuePair<string, string>("client_secret", client_secret)
        });

        var response = await _httpClient.PostAsync(uri, requestContent);
        if( response.StatusCode != HttpStatusCode.OK){
            throw new KeycloakClientFailedAuthenticationException();
        }
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<KeycloakAuthResponseDto>(responseContent)!.access_token;
    }
    
    //Assignes a role to the Keycloak user
    public async Task AssignKeycloakUserRole(User user)
    {
        var role = await GetKeycloakRoleByName(user.Role.ToString());
        Uri uri = new($"{keycloakHost}/admin/realms/{realm}/users/{user.Id}/role-mappings/clients/{client_uuid}");
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = uri,
            Headers = {
                {HttpRequestHeader.Authorization.ToString(), $"Bearer {await GetKeycloakClientToken()}"},
            },
            Content = new StringContent(role, Encoding.UTF8, "application/json")
        };
        var response = await _httpClient.SendAsync(httpRequestMessage);
        if(!response.IsSuccessStatusCode){
            throw new KeycloakUserRoleAssignmentException(user.Role.ToString(), user.Username);
        }
    }
    
    //Returns a role to be used in the AssignKeycloakUserRole function
    private async Task<string> GetKeycloakRoleByName(string roleName)
    {
        Uri uri = new($"{keycloakHost}/admin/realms/{realm}/clients/{client_uuid}/roles/{roleName}");
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = uri,
            Headers = {
                {HttpRequestHeader.Authorization.ToString(), $"Bearer {await GetKeycloakClientToken()}"},
            },
        };
        var response = await _httpClient.SendAsync(httpRequestMessage);
        string responseContent = await response.Content.ReadAsStringAsync();
        KeycloakRoleDto dto = JsonSerializer.Deserialize<KeycloakRoleDto>(responseContent)!;
        List<KeycloakRoleDto> list = [ dto ];
        string s = JsonSerializer.Serialize(list);
        return s;
    }

    public async Task SetUserEnabled(string username, bool enabled)
    {
        Uri uri = new($"{keycloakHost}/admin/realms/{realm}/users/{await GetKeycloakUserId(username)}");
        var payload = new {
            enabled
        };
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = uri,
            Headers = {
                {HttpRequestHeader.Authorization.ToString(), $"Bearer {await GetKeycloakClientToken()}"},
            },
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };
        var response = await _httpClient.SendAsync(httpRequestMessage);
        if( response.StatusCode != HttpStatusCode.NoContent){
            throw new KeycloakUserDisableFailedException(username);
        }
    }
    public async Task UpdateKeycloakUser(User user)
    {
        Uri uri = new($"{keycloakHost}/admin/realms/{realm}/users/{user.Id}");
        var keycloakUser = new {
            firstName = user.Name,
            lastName = user.Lastname,
            email = user.Email,
            emailVerified = true
        };
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = uri,
            Headers = {
                {HttpRequestHeader.Authorization.ToString(), $"Bearer {await GetKeycloakClientToken()}"},
            },
            Content = new StringContent(JsonSerializer.Serialize(keycloakUser), Encoding.UTF8, "application/json")
        };
        var response = await _httpClient.SendAsync(httpRequestMessage);
        if( !response.IsSuccessStatusCode){
            throw new KeycloakUserUpdateException(user.Username);
        }
        return;
    }
}
