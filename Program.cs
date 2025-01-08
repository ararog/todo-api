using System.Reflection;
using System.Text;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using TodoApi.Models;
using UsersService.Models;
using OpenTelemetry.Metrics;
using Npgsql;
using TodoApi.Controllers;

var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
NpgsqlLoggingConfiguration.InitializeLogging(loggerFactory);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithMetrics(builder =>
    {
      builder.AddPrometheusExporter();

      builder.AddMeter("Microsoft.AspNetCore.Hosting",
                       "Microsoft.AspNetCore.Server.Kestrel");
      builder.AddView("http.server.request.duration",
          new ExplicitBucketHistogramConfiguration
          {
            Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05,
                       0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 }
          });
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
     {
       options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
       options.Audience = builder.Configuration["Auth0:Audience"];
       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
       {
         ValidAudience = builder.Configuration["Auth0:Audience"],
         ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}",
         RoleClaimType = $"{builder.Configuration["Auth0:TokenNamespace"]}claims/roles",
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PbnX6fqF8fZTUNPVcfG09K95gOfFH3sC"))
       };
     });

builder.Services
    .AddAuthorization(options =>
    {
      options.AddPolicy("User", policy => policy.RequireClaim("User"));
      options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
    });

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: "AllowOrigins",
                    policy =>
                    {
                      policy.WithOrigins(["https://localhost:7138", "http://localhost:3000"]).AllowAnyHeader().AllowAnyMethod();
                    });
});

var myOptions = new MyRateLimitOptions();
builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);
var slidingPolicy = "sliding";

builder.Services.AddRateLimiter(_ => _
    .AddSlidingWindowLimiter(policyName: slidingPolicy, options =>
    {
      options.PermitLimit = myOptions.PermitLimit;
      options.Window = TimeSpan.FromSeconds(myOptions.Window);
      options.SegmentsPerWindow = myOptions.SegmentsPerWindow;
      options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
      options.QueueLimit = myOptions.QueueLimit;
    }));

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddSingleton<TodoApi.Utils.DapperContext>();
builder.Services.AddSingleton<TodoApi.Utils.Database>();
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

app.MigrateDatabase();
app.MapPrometheusScrapingEndpoint();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseCors("AllowOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
