public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var databaseService = scope.ServiceProvider.GetRequiredService<TodoApi.Utils.Database>();

            try
            {
                databaseService.CreateDatabase("Todo");
            }
            catch
            {
                //log errors or ...
                throw;
            }
        }

        return host;
    }
}