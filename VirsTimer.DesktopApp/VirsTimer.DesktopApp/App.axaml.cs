using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using VirsTimer.Core.Services;
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.Views;

namespace VirsTimer.DesktopApp
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public IServiceProvider ServiceProvider { get; private set; } = null!;

        public IConfiguration Configuration { get; private set; } = null!;

        private void ConfigureServices(IServiceCollection services)
        {
            var fileSolvesService = new FileSolvesService();

            services.AddSingleton<IPastSolvesGetter>(fileSolvesService);
            services.AddSingleton<ISolvesSaver>(fileSolvesService);
            services.AddSingleton<IEventsGetter>(fileSolvesService);
            services.AddSingleton<ISessionsManager>(fileSolvesService);
            services.AddSingleton<IScrambleGenerator>(new RandomScrambleGenerator());
            services.AddTransient(services => 
                new MainWindowViewModel(
                    "3x3x3",
                    services.GetRequiredService<IPastSolvesGetter>(),
                    services.GetRequiredService<ISolvesSaver>(),
                    services.GetRequiredService<IScrambleGenerator>(),
                    services.GetRequiredService<ISessionsManager>()));
            services.AddHttpClient();
            services.AddTransient<MainWindow>();
        }
    }
}
