using System;
using System.Diagnostics;
using System.Windows;
using DesktopApp.Models;
using DesktopApp.Services;
using DesktopApp.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Reactive.Bindings;
using Reactive.Bindings.Schedulers;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ConfigureServices();
            SetReactivePropertyScheduler();
            InitializeComponent();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton<LdSearcher>()
                .AddSingleton<AnkiCardCreator>();

            services
                .AddSingleton<MainScreenViewModel>()
                .AddSingleton<LdBodyViewModel>();

            services
                .AddSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>()
                .AddSingleton<IViewLocator, ViewLocator>()
                .AddSingleton<IDialogService, DialogService>();

            DISource.SetServiceProvider(
                services.BuildServiceProvider()
            );
        }

        private void SetReactivePropertyScheduler()
        {
            ReactivePropertyScheduler.SetDefault(new ReactivePropertyWpfScheduler(Dispatcher));
        }
    }
}