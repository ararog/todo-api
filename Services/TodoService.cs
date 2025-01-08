using Dapper;
using UsersService.Models;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService : IApiService<TodoItem>
{
  string _dbSettings;

  public TodoService(
      IOptions<DatabaseSettings> databaseSettings)
  {
    _dbSettings = databaseSettings.Value.FormatConnectionString();
  }

  public Task<IEnumerable<TodoItem>> GetAll() =>
       throw new NotImplementedException();

  public async Task<IEnumerable<TodoItem>> GetByUserId(string id)
  {
    using (var connection = new NpgsqlConnection(_dbSettings))
    {
      return await connection.QueryAsync<TodoItem>(
        "SELECT * FROM todoitem WHERE userid = @userid", new { userId = id });
    }
  }

  public async Task<TodoItem?> Get(int id)
  {
    using (var connection = new NpgsqlConnection(_dbSettings))
    {
      var sql = "SELECT * FROM todoitem WHERE id = @Id";
      return await connection.QueryFirstOrDefaultAsync<TodoItem>(sql, new { Id = id });
    }
  }

  public async Task Create(TodoItem item)
  {
    using (var connection = new NpgsqlConnection(_dbSettings))
    {
      var sql = @"INSERT INTO todoitem (title, description, completed, userid) 
        VALUES (@Title, @Description, @Completed, @UserId)";
      await connection.ExecuteAsync(sql, item);
    }
  }

  public async Task Update(TodoItem item)
  {
    var sql = @"UPDATE todoitem SET 
      description = @Description, 
      completed = @Completed, 
      completedat = @CompletedAt
    WHERE Id = @Id";

    if (item.Completed)
    {
      item.CompletedAt = DateTime.Now;
    }

    using (var connection = new NpgsqlConnection(_dbSettings))
    {
      await connection.ExecuteAsync(sql, item);
    }
  }

  public async Task Remove(int id)
  {
    var sql = "DELETE FROM todoitem WHERE id = @Id";
    using (var connection = new NpgsqlConnection())
    {
      await connection.ExecuteAsync(sql, new { ID = id });
    }
  }
}