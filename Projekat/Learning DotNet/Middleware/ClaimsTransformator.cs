using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace Learning_DotNet.Middleware;

public class ClaimsTransformator : IClaimsTransformation
{
    public ClaimsTransformator(IConfiguration configuration)
    {
        _client_id = configuration["Keycloak:Resource"];
    }
    private readonly string? _client_id;
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity!;
        
        if (claimsIdentity!.IsAuthenticated && claimsIdentity.HasClaim((claim) => claim.Type == "resource_access"))
        {
            var resourceAccessClaim = claimsIdentity.FindFirst((claim) => claim.Type == "resource_access");
            var resourceAccessAsJson = JsonSerializer.Deserialize<JsonElement>(resourceAccessClaim!.Value);
            var rolesJson = resourceAccessAsJson.GetProperty(_client_id!).GetProperty("roles");
            var roles = rolesJson.Deserialize<string[]>();
            foreach (var role in roles!)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }                 
        }
        return Task.FromResult(principal);
    }
}
