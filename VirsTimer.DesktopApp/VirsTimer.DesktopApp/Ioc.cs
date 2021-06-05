using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.IO.Abstractions;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Helpers;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Services;
using VirsTimer.Core.Services.Cache;
using VirsTimer.Core.Services.Events;
using VirsTimer.Core.Services.Login;
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
        private static readonly IServiceCollection ServiceDescriptors;

        public static IServiceProvider Services { get; private set; }
        public static IConfiguration Configuration { get; private set; } = null!;

        static Ioc()
        {
            BuildConfiguration();

            ServiceDescriptors = new ServiceCollection();
            ConfigureServices(ServiceDescriptors);
            Services = ServiceDescriptors.BuildServiceProvider();
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
            services.AddHttpClient(
                Options.DefaultName,
                client => client.BaseAddress = new Uri(Path.Combine(Server.Address)));

            services.AddHttpClient(
                Server.Endpoints.Scrambles,
                client => client.BaseAddress = new Uri(Path.Combine(Server.Address, Server.Endpoints.Scrambles)));

            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<FileHelper>();
            services.AddSingleton<IApplicationCacheSaver, ApplicationCacheSaver>();
            services.AddSingleton<IEventsRepository, ServerEventsRepository>();
            services.AddSingleton<ISessionRepository, ServerSessionsRepository>();
            services.AddSingleton<ISolvesRepository, ServerSolvesRepository>();
            services.AddSingleton<IScrambleGenerator, ServerScrambleGenerator>();
            services.AddSingleton<ILoginRepository, ServerLoginRepository>();
            services.AddHttpClient();
        }

        public static void AddUserClient(IUserClient userClient)
        {
            ServiceDescriptors.AddSingleton<IUserClient>(userClient);
            Services = ServiceDescriptors.BuildServiceProvider();
        }
    }
}