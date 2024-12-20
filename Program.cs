using System.Net;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// builder.WebHost.ConfigureKestrel((context, serverOptions) =>
// {
//     serverOptions.Listen(IPAddress.Any, 80);
// });

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
     {
         c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
         c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidAudience = builder.Configuration["Auth0:Audience"],
             ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}",
             RoleClaimType = $"{builder.Configuration["Auth0:TokenNamespace"]}claims/roles"
         };
     });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy => policy.RequireClaim("User"));
    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
});

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton<TodoApi.Utils.DapperContext>();
builder.Services.AddSingleton<TodoApi.Utils.Database>();
builder.Services.AddSingleton<TodoApi.Services.UsersService>();
builder.Services.AddSingleton<TodoApi.Services.TodoService>();
builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
        .AddFluentMigratorCore()
        .ConfigureRunner(c => c.AddPostgres()
            .WithGlobalConnectionString(
                string.Format(
                    builder.Configuration.GetSection("Database").GetValue<string>("MasterConnectionString"),
                    Environment.GetEnvironmentVariable("DB_HOST"),
                    Environment.GetEnvironmentVariable("DB_USER"),
                    Environment.GetEnvironmentVariable("DB_PASSWORD")
                )
            )
            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MigrateDatabase();

app.Run();
