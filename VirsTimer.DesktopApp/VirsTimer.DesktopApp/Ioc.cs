using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using VirsTimer.Core.Services;

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

            services.AddSingleton<IPastSolvesGetter>(fileSolvesService);
            services.AddSingleton<ISolvesSaver>(fileSolvesService);
            services.AddSingleton<IEventsGetter>(fileSolvesService);
            services.AddSingleton<ISessionsManager>(fileSolvesService);
            services.AddSingleton<IScrambleGenerator>(new RandomScrambleGenerator());
            services.AddHttpClient();
        }
    }
}
