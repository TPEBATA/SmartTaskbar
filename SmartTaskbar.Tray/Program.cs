using Microsoft.Extensions.DependencyInjection;
using SmartTaskbar.Application;
using SmartTaskbar.Engines;
using SmartTaskbar.Engines.Interfaces;
using SmartTaskbar.PlatformInvoke.FileSystem;
using SmartTaskbar.Tray.Languages;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace SmartTaskbar.Tray
{
    static class Program
    {
        private static ServiceProvider _serviceProvider;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Use a mutex to ensure single instance
            using (new Mutex(true, "{959d3545-aa5c-42a8-a327-6e2c079daa94}", out var createNew))
            {
                if (createNew)
                {
                    // Start a tray instead of a WinForm to reduce memory usage
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    DependencyInjection();

                    Application.Run(_serviceProvider.GetService<MainNotifyIcon>());
                }
            }
        }

        private static void DependencyInjection()
        {
            var serivceCollection = new ServiceCollection();
            ConfigureServices(serivceCollection);

            _serviceProvider = serivceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection serivceCollection)
        {
            // todo add Services;
            serivceCollection.AddSingleton<IContainer, Container>();
            serivceCollection.AddSingleton<MainNotifyIcon>();
            serivceCollection.AddSingleton<CultureResource>();
            serivceCollection.AddSingleton<IUserConfigService,UserConfigService>();
            serivceCollection.AddSingleton<UserConfigEngine>();

        }
    }
}