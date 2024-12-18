using System.Text.Json.Serialization;

namespace UsersService.Models;

public class TodoItem
{
  [JsonPropertyName("id")]
  public string? Id { get; set; }

  [JsonPropertyName("text")]
  public required string Text { get; set; }

  [JsonPropertyName("description")]
  public required string Description { get; set; }

  [JsonPropertyName("userId")]
  public required string UserId { get; set; }
}