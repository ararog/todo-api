using Dapper;
using UsersService.Models;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService : IApiService<TodoItem>
{
  IDbConnection _connection;

  public TodoService(
      IOptions<DatabaseSettings> databaseSettings)
  {
    _connection = new NpgsqlConnection(
        string.Format(databaseSettings.Value.ConnectionString,
        Environment.GetEnvironmentVariable("DB_NAME"),
        Environment.GetEnvironmentVariable("DB_USER"),
        Environment.GetEnvironmentVariable("DB_PASSWORD")));
  }

  public Task<IEnumerable<TodoItem>> GetAll() =>
       throw new NotImplementedException();

  public async Task<IEnumerable<TodoItem>> GetAll(int id) =>
      await _connection.QueryAsync<TodoItem>("SELECT * FROM TodoItem WHERE userId = @userId", new { userId = id });

  public async Task<TodoItem?> Get(string id)
  {
    var sql = "SELECT * FROM TodoItem WHERE todoItemId = @userId";
    return await _connection.QueryFirstOrDefaultAsync(sql, new { userId = id });
  }

  public async Task Create(TodoItem item)
  {
    var sql = "INSERT INTO TodoItem (Text, Description) VALUES (@Text, @Description)";
    await _connection.ExecuteAsync(sql, item);
  }

  public async Task Update(TodoItem item)
  {
    var sql = "UPDATE TodoItem SET (Text = @Text, Description = @Description)  WHERE Id = @Id";
    await _connection.ExecuteAsync(sql, item);
  }

  public async Task Remove(string id)
  {
    var sql = "DELETE FROM TodoItem WHERE Id = @Id";
    await _connection.ExecuteAsync(sql, new { ID = id });
  }
}