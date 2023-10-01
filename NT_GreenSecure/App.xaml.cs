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
            DependencyService.Register<DAO_Credentials>();
            DependencyService.Register<DAO_Users>();
            MainPage = new AppShell();
            // Preferences.Set("token_expiry", DateTime.UtcNow.AddHours(48).Ticks);
            // Récupération de l'IdUser et de la date d'expiration du token
            int userId = Preferences.Get("IdUser", -1);
            long ticks = Preferences.Get("token_expiry", 0L);

            DateTime tokenExpiry = new DateTime(ticks);

            if (userId != -1)
            {
                if (DateTime.UtcNow < tokenExpiry)
                {
                    // Vérification supplémentaire pour voir si l'utilisateur est toujours valide, si nécessaire

                    Shell.Current.GoToAsync("//HomePage");

                }
                else
                {
                    Preferences.Remove("token_expiry");
                    App.Current.MainPage.DisplayAlert("Session expirée", "Veuillez vous reconnecter", "OK");

                  
                    Shell.Current.GoToAsync("//LoginPage");
                }
            }
            else
            {
                Shell.Current.GoToAsync("//LoginPage");

            }
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}