using NT_GreenSecure.Models;
using NT_GreenSecure.Services;
using NT_GreenSecure.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NT_GreenSecure
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjczNDI3MUAzMjMzMmUzMDJlMzBTdXVxQ3FxTnEyM3VPTlhpSTdEZ0tKRm9EdGxJQVNZSGhNUFVVcTJPZ2FnPQ==");
            DependencyService.Register<DAO_Credentials>();
            DependencyService.Register<DAO_Users>();
            MainPage = new AppShell(); // Set the main page
        }

        protected override void OnStart()
        {
            if (MainPage is Shell shellPage)
            {
                int userId = Preferences.Get("IdUser", -1);
                long ticks = Preferences.Get("token_expiry", 0L);
                DateTime tokenExpiry = new DateTime(ticks);
                bool IsTokenExpired = DateTime.UtcNow >= tokenExpiry;

                if (userId != -1)
                {
                    if (IsTokenExpired)
                    {
                        Preferences.Remove("token_expiry");
                        Preferences.Set("IdUser", -1);
                        App.Current.MainPage.DisplayAlert("Session expirée", "Veuillez vous reconnecter", "OK").ContinueWith(t =>
                        {
                            if (t.IsCompleted)
                                shellPage.GoToAsync($"//{nameof(LoginPage)}");
                        });
                    }
                    else
                    {
                        shellPage.GoToAsync($"//{nameof(HomePage)}");
                    }
                }
                else
                {
                    shellPage.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}