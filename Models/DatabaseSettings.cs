namespace TodoApi.Models;

public class DatabaseSettings
{
  public string ConnectionString { get; set; } = null!;

  public string FormatConnectionString()
  {
    return string.Format(ConnectionString,
        Environment.GetEnvironmentVariable("DB_HOST"),
        Environment.GetEnvironmentVariable("DB_PORT"),
        Environment.GetEnvironmentVariable("DB_USER"),
        Environment.GetEnvironmentVariable("DB_PASSWORD"));
  }
}