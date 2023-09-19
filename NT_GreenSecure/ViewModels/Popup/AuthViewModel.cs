using NT_GreenSecure.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NT_GreenSecure.ViewModels.Popup
{
    public class AuthViewModel : BaseViewModel
    {

        public string Email { get; set; }
        public string Password { get; set; }

        public Command LoginCommand { get; }

        Auth auth;

        public event Action RequestClosePopup;

        public AuthViewModel(){
            Title = "Auth";
            auth = new Auth();
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            bool isAuthenticated = await auth.AuthenticateAsync(Email, Password);

            if (isAuthenticated)
            {
                // Demander à fermer le popup
                RequestClosePopup?.Invoke();

                await Shell.Current.GoToAsync($"//{nameof(VaultPage)}");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Erreur", "Email ou mot de passe incorrect", "OK");
            }
        }
    }
}
