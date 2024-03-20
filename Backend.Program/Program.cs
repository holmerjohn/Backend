using Backend.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

                var loanProcessor = host.Services.GetRequiredService<ILoanProcessor>();
                await loanProcessor.ProcessLoanFilesAsync();
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
                    services.AddScoped<ILoanProcessor, LoanProcessor>();

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
                ConnectionString = configuration.GetConnectionString("BackendDb")
            };
        }
    }
}
