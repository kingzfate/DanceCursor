using DanceCursor.Client.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using System.Windows.Threading;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace DanceCursor.Client
{
    /// <inheritdoc />
    public partial class App
    {
        private IConfigurationRoot _configuration = null!;

        public App()
        {
            Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
        }

        /// <inheritdoc />
        protected override IContainerExtension CreateContainerExtension()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddAdvancedDependencyInjection();


            var container = new UnityContainer();
            container.BuildServiceProvider(serviceCollection);

            return new UnityContainerExtension(container);
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        }

        /// <inheritdoc />
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}