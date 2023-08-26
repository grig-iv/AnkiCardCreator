using System.Windows;
using DesktopApp.Models;
using DesktopApp.Views;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Unity;
using ReactiveUI;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                // services
                .RegisterSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>()

                // models
                .RegisterInstance(new LdSearcher(RxApp.MainThreadScheduler))
                .RegisterSingleton<AnkiCardCreator>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }
}