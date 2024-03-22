using Backend.Data;
using Backend.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Backend.Program
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (IHost host = CreateHostBuilder(args).Build())
            {
                /*
                 * Make sure the database exists, and is initialized
                 * */
                var databaseService = host.Services.GetRequiredService<IDatabaseService>();
                databaseService.Initialize();

                var backendConfiguration = host.Services.GetRequiredService<BackendConfiguration>();
                var actionStore = host.Services.GetRequiredService<IActionStore>();
                var actionProcessor = host.Services.GetRequiredService<IEntityActionProcessor>();

                var filePath = Path.Combine(backendConfiguration.CurrentDirectory, "actions.json");
                
                IEnumerable<EntityAction> actions = null;
                if (File.Exists(filePath))
                {
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        actions = await actionStore.GetActionsAsync(fileStream);
                    }
                }
                else 
                {
                    actions = Enumerable.Empty<EntityAction>();
                }
                await actionProcessor.ProcessEntityActionsAsync(actions);
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = CreateConfiguration(args);
            var backendConfiguration = GetBackendConfiguration(configuration);

            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddConsole();
                    });

                    services.AddSingleton(backendConfiguration);
                    services.AddBackendDataServices(backendConfiguration);
                    services.AddScoped<IEntityActionProcessor, EntityActionProcessor>();
                    services.AddScoped<IActionStore, ActionStore>();

                });
        }

        private static IConfiguration CreateConfiguration(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                {"-dir", "directory" }
            };

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile("local.settings.json", true, false)
                .AddCommandLine(args, switchMappings);

            return configuration.Build();
        }

        private static BackendConfiguration GetBackendConfiguration(IConfiguration configuration)
        {
            bool.TryParse(configuration["EnableSensitiveSqlLogging"], out bool enableSensitiveSqlLogging);
            return new BackendConfiguration()
            {
                EnableSensitiveSqlLogging = enableSensitiveSqlLogging,
                ConnectionString = configuration.GetConnectionString("BackendDb"),
                CurrentDirectory = configuration["directory"] ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            };
        }
    }
}
