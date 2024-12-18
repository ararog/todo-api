using Dapper;
using UsersService.Models;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using TodoApi.Models;

namespace TodoApi.Services;

public class UsersService
{
    IDbConnection _connection;

    public UsersService(
        IOptions<DatabaseSettings> databaseSettings)
    {
        _connection = new NpgsqlConnection(
            databaseSettings.Value.ConnectionString);
    }

    public async Task<IEnumerable<User>> Get() =>
        await _connection.QueryAsync<User>("select * from users");

    public async Task<User?> Get(string id)
    {
        var sql = "SELECT * FROM Users WHERE userId = @userId";
        return await _connection.QueryFirstOrDefaultAsync(sql, new { userId = id });
    }

    public async Task Create(User newUser)
    {
        var sql = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";
        await _connection.ExecuteAsync(sql, newUser);
    }

    public async Task Update(User updatedUser)
    {
        var sql = "UPDATE Users SET (Name = @Name, Email = @Email)  WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, updatedUser);
    }

    public async Task Remove(string id)
    {
        var sql = "DELETE FROM Users WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { ID = id });
    }
}