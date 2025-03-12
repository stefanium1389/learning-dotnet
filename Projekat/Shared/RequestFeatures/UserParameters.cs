namespace Shared.RequestFeatures;

public class UserParameters : RequestParameters
{
    public UserParameters() => OrderBy = "Name";
    public string Name {get; set;} = string.Empty;
    public string Lastname {get; set;} = string.Empty;
    public string Role {get; set;} = string.Empty;
}
