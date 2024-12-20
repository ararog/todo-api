using System.Text.Json.Serialization;

namespace UsersService.Models;

public class TodoItem
{
  [JsonPropertyName("id")]
  public string? Id { get; set; }

  [JsonPropertyName("title")]
  public required string Title { get; set; }

  [JsonPropertyName("description")]
  public required string Description { get; set; }

  [JsonPropertyName("completed")]
  public required bool Completed { get; set; }

  [JsonPropertyName("createdAt")]
  public required DateTime CreatedAt { get; set; }

  [JsonPropertyName("completedAt")]
  public required DateTime CompletedAt { get; set; }

  [JsonPropertyName("userId")]
  public required string UserId { get; set; }
}