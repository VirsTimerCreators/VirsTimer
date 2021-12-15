using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Multiplayer;
using VirsTimer.Core.Services;
using VirsTimer.Core.Services.Cache;
using VirsTimer.Core.Services.Events;
using VirsTimer.Core.Services.Login;
using VirsTimer.Core.Services.Register;
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
                HttpClientNames.Blank,
                client => client.BaseAddress = new Uri(Path.Combine(Server.Address)));


            services.AddSingleton<IHttpResponseHandler, HttpResponseHandler>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IApplicationCacheSaver, ApplicationCacheFileIO>();
            services.AddSingleton<IUserClient, UserClient>();
            services.AddSingleton<ILoginRepository, LoginServerRepository>();
            services.AddSingleton<IRegisterRepository, RegisterServerRepository>();
            services.AddSingleton<ICustomScrambleGeneratorsCollector, AssemblyCustomScrambleGeneratorsCollector>();
            services.AddHttpClient();

            Services = ServiceDescriptors.BuildServiceProvider();
        }

        public static async Task AddApplicationCacheAsync(bool serverSide = true)
        {
            var fileSystem = Services.GetService<IFileSystem>()!;
            var fileName = serverSide ? "ServerCache.json" : null;
            var applicationCacheFileIO = new ApplicationCacheFileIO(fileSystem, fileName);
            var cache = await applicationCacheFileIO.LoadCacheAsync().ConfigureAwait(false);

            ServiceDescriptors.AddSingleton<IApplicationCacheSaver>(applicationCacheFileIO);
            ServiceDescriptors.AddSingleton(cache);
        }

        public static void ConfigureLocalServices()
        {
            ServiceDescriptors.AddSingleton<IEventsRepository, EventsFileRepository>();
            ServiceDescriptors.AddSingleton<ISessionsRepository, SessionsFileRepository>();
            ServiceDescriptors.AddSingleton<ISolvesRepository, SolvesFileRepository>();
            ServiceDescriptors.AddSingleton<IScrambleGenerator, ScrambleServerGenerator>();
            Services = ServiceDescriptors.BuildServiceProvider();
        }

        public static void ConfigureServerServices(IUserClient userClient)
        {
            ServiceDescriptors.AddHttpClient(
                HttpClientNames.UserAuthorized,
                client =>
                {
                    client.BaseAddress = new Uri(Path.Combine(Server.Address));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userClient.Jwt);
                });

            ServiceDescriptors.AddSingleton<IEventsRepository, EventsServerRepository>();
            ServiceDescriptors.AddSingleton<ISessionsRepository, SessionsServerRepository>();
            ServiceDescriptors.AddSingleton<ISolvesRepository, SolvesServerRepository>();
            ServiceDescriptors.AddSingleton<IScrambleGenerator, ScrambleServerGenerator>();
            ServiceDescriptors.AddTransient<IRoomsService, RoomServerService>();
            ServiceDescriptors.AddSingleton(userClient);
            Services = ServiceDescriptors.BuildServiceProvider();
        }
    }
}