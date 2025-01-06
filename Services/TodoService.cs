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
    _connection = new NpgsqlConnection(databaseSettings.Value.FormatConnectionString());
  }

  public Task<IEnumerable<TodoItem>> GetAll() =>
       throw new NotImplementedException();

  public async Task<IEnumerable<TodoItem>> GetByUserId(string id) =>
      await _connection.QueryAsync<TodoItem>(
        "SELECT * FROM todoitem WHERE userid = @userid", new { userId = id });

  public async Task<TodoItem?> Get(int id)
  {
    var sql = "SELECT * FROM todoitem WHERE id = @Id";
    return await _connection.QueryFirstOrDefaultAsync(sql, new { Id = id });
  }

  public async Task Create(TodoItem item)
  {
    var sql = @"INSERT INTO todoitem (title, description, completed) 
      VALUES (@Title, @Description, @Completed)";
    await _connection.ExecuteAsync(sql, item);
  }

  public async Task Update(TodoItem item)
  {
    var sql = @"UPDATE todoitem SET (
      title = @Title, 
      description = @Description, 
      completed = @Completed, 
      completedat = @CompletedAt
    ) WHERE Id = @Id";

    if (item.Completed)
    {
      item.CompletedAt = DateTime.Now;
    }
    await _connection.ExecuteAsync(sql, item);
  }

  public async Task Remove(int id)
  {
    var sql = "DELETE FROM todoitem WHERE id = @Id";
    await _connection.ExecuteAsync(sql, new { ID = id });
  }
}