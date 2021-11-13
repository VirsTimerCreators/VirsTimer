﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http.Headers;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Handlers;
using VirsTimer.Core.Helpers;
using VirsTimer.Core.Models.Authorization;
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
                HttpClientNames.Blank,
                client => client.BaseAddress = new Uri(Path.Combine(Server.Address)));


            services.AddSingleton<IHttpResponseHandler, HttpResponseHandler>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<FileHelper>();
            services.AddSingleton<IApplicationCacheSaver, ApplicationCacheSaver>();
            services.AddSingleton<IUserClient, UserClient>();
            services.AddSingleton<ILoginRepository, ServerLoginRepository>();
            services.AddSingleton<IRegisterRepository, ServerRegisterRepository>();
            services.AddHttpClient();
        }

        public static void ConfigureLocalServices()
        {
            // todo ...
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
            ServiceDescriptors.AddSingleton<ISessionRepository, ServerSessionsRepository>();
            ServiceDescriptors.AddSingleton<ISolvesRepository, ServerSolvesRepository>();
            ServiceDescriptors.AddSingleton<IScrambleGenerator, ServerScrambleGenerator>();
            ServiceDescriptors.AddSingleton(userClient);
            Services = ServiceDescriptors.BuildServiceProvider();
        }
    }
}