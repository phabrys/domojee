﻿using Domojee.Services.SettingsServices;
using Domojee.Views;
using Jeedom;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Domojee
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion App settings
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // content may already be shell when resuming
            if ((Window.Current.Content as Shell) == null)
            {
                // setup hamburger shell
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                Window.Current.Content = new Shell(nav);
            }
            return Task.CompletedTask;
        }

        // runs only when not restored from state
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            ConfigurationViewModel config = new ConfigurationViewModel();
            if (config.Populated)
            {
                var req = RequestViewModel.Instance;
                if (await req.DownloadAll() != null)
                {
                    NavigationService.Navigate(typeof(Views.ConnectPage));
                }

                await Task.Delay(TimeSpan.FromSeconds(2));
                NavigationService.Navigate(typeof(DashboardPage));
            }
            else
            {
                NavigationService.Navigate(typeof(Views.ConnectPage));
            }

            return;// Task.CompletedTask;
        }
    }
}