using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimCodes.Mtd.Vat.App.Extensions;
using TimCodes.Mtd.Vat.App.Services;
using TimCodes.Mtd.Vat.Core.Authorisation;
using TimCodes.Mtd.Vat.Core.Configuration;
using TimCodes.Mtd.Vat.Core.Extensions;

namespace TimCodes.Mtd.Vat.App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();
            var config = Configure();

            ConfigureServices(services, config);

            using var sp = services.BuildServiceProvider();

            Application.Run(sp.GetRequiredService<MainForm>());
        }

        private static IConfiguration Configure()
        {
            var config = new ConfigurationBuilder();
            config.SetBasePath(Directory.GetCurrentDirectory());

            config.AddJsonFile("appsettings.json");
            config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);
            config.AddUserSecrets<MainForm>();

            return config.Build();
        }

        private static void ConfigureServices(ServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(config);
            services.AddLogging(configure => configure.AddConsole().AddDebug());
            services.AddVatServices(config);
            services.AddScoped<FormService>();
            services.AddAllScoped<Form>();
        }
    }
}