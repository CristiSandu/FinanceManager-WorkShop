using FinanceManager.Helpers;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinanceManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Settings.SetTheme();
            VersionTracking.Track();

            var firstLaunch = VersionTracking.IsFirstLaunchEver;

            if (firstLaunch)
                MainPage = new NavigationPage(new Views.WelcomePage());
            else
                MainPage = new NavigationPage(new Views.MainTabbedPage());
        }

        protected override void OnStart()
        {
            OnResume();
        }

        protected override void OnSleep()
        {
            Settings.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            Settings.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Settings.SetTheme();
            });
        }
    }
}
