using System.Text.Json.Serialization;

namespace UsersService.Models;

public class User
{
  [JsonPropertyName("id")]
  public string? Id { get; set; }

  [JsonPropertyName("email")]
  public string? Email
  {
    get; set;
  }

  [JsonPropertyName("password")]
  public string? Password
  {
    get; set;
  }

  [JsonPropertyName("auth0Id")]
  public string? Auth0Id
  {
    get; set;
  }

  [JsonPropertyName("permissionLevel")]
  public string? PermissionLevel
  {
    get; set;
  }
}