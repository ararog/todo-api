using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UsersService.Models;

[Table("todoitem")]

public class TodoItem
{
  [JsonPropertyName("id")]
  [Column("id")]
  public string? Id { get; set; }

  [JsonPropertyName("title")]
  [Column("title")]
  public required string Title { get; set; }

  [JsonPropertyName("description")]
  [Column("description")]
  public required string Description { get; set; }

  [JsonPropertyName("completed")]
  [Column("completed")]
  public required bool Completed { get; set; }

  [JsonPropertyName("createdAt")]
  [Column("createdat")]
  public required DateTime CreatedAt { get; set; }

  [JsonPropertyName("completedAt")]
  [Column("completedat")]
  public required DateTime CompletedAt { get; set; }

  [JsonPropertyName("userId")]
  [Column("userid")]
  public required string UserId { get; set; }
}