using System.Text.Json.Serialization;

namespace UsersService.Models;

public class TodoItem
{
  [JsonPropertyName("id")]
  public string? Id { get; set; }

}