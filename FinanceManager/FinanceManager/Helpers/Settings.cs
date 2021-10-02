using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManager.Helpers
{
    public static class Settings
    {
        const int theme = 0;

        public static int Theme
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(Theme), theme);
            set => Xamarin.Essentials.Preferences.Set(nameof(Theme), value);
        }


        public static void SetTheme()
        {
            switch (Theme)
            {
                //default
                case 0:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Unspecified;
                    break;
                //light
                case 1:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Light;
                    break;
                //dark
                case 2:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Dark;
                    break;
            }
        }
    }
}
