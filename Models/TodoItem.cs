using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UsersService.Models;

[Table("todoitem")]

public class TodoItem
{
  [JsonPropertyName("id")]
  [Column("id")]
  public int? Id { get; set; }

  [JsonPropertyName("title")]
  [Column("title")]
  public string? Title { get; set; }

  [JsonPropertyName("description")]
  [Column("description")]
  public required string Description { get; set; }

  [JsonPropertyName("completed")]
  [Column("completed")]
  public bool Completed { get; set; }

  [JsonPropertyName("createdAt")]
  [Column("createdat")]
  public DateTime CreatedAt { get; set; }

  [JsonPropertyName("completedAt")]
  [Column("completedat")]
  public DateTime CompletedAt { get; set; }

  [JsonPropertyName("userId")]
  [Column("userid")]
  public string? UserId { get; set; }
}