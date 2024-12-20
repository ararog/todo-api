namespace TodoApi.Utils;

using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

public class DapperContext
{
  private readonly IConfiguration _configuration;
  public DapperContext(IConfiguration configuration)
  {
    _configuration = configuration;
  }
  public IDbConnection CreateConnection()
      => new NpgsqlConnection(_configuration.GetSection("Database").GetValue<string>("ConnectionString"));
  public IDbConnection CreateMasterConnection()
      => new NpgsqlConnection(string.Format(_configuration.GetSection("Database").GetValue<string>("MasterConnectionString"),
                    Environment.GetEnvironmentVariable("DB_HOST"),
                    Environment.GetEnvironmentVariable("DB_USER"),
                    Environment.GetEnvironmentVariable("DB_PASSWORD")));
}