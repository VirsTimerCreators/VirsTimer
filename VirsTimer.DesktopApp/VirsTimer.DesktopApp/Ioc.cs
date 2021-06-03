using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.IO.Abstractions;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Services;
using VirsTimer.Core.Services.Events;
using VirsTimer.Core.Services.Scrambles;
using VirsTimer.Core.Services.Sessions;
using VirsTimer.Core.Services.Solves;

namespace VirsTimer.DesktopApp
{
    /// <summary>
    /// Invesrion of control.
    /// </summary>
    public static class Ioc
    {
        public static IServiceProvider Services { get; }
        public static IConfiguration Configuration { get; private set; } = null!;

        static Ioc()
        {
            BuildConfiguration();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
        }

        public static TService GetService<TService>() where TService : class
        {
            return Services.GetRequiredService<TService>();
        }

        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var fileSolvesService = new FileSolvesService();

            services.AddHttpClient(
                Server.ScrambleEndpoint,
                client => client.BaseAddress = new Uri(Path.Combine(Server.Address, Server.ScrambleEndpoint)));

            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ISolvesRepository, FileSolvesRepository>();
            services.AddSingleton<IEventsRepository, FileEventsRepository>();
            services.AddSingleton<ISessionsManager>(fileSolvesService);
            services.AddSingleton<IScrambleGenerator, ServerScrambleGenerator>();
            services.AddHttpClient();
        }
    }
}
