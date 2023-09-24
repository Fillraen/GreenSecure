using NT_GreenSecure.Models;
using NT_GreenSecure.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NT_GreenSecure.ViewModels
{
    public class VaultViewModel : BaseViewModel
    {
        

        private ObservableCollection<Credentials> _credentials;
        public ObservableCollection<Credentials> Credentials
        {
            get => _credentials;
            set => SetProperty(ref _credentials, value);
        }

        private readonly DAO_Credentials _daoCredentials;
        private readonly DAO_Users _daoUsers;

        private int userId;
        private User connectedUser;

        public VaultViewModel()
        {
            Title = "Vault";
            _daoCredentials = new DAO_Credentials();
            _daoUsers = new DAO_Users();
            userId = Preferences.Get("IdUser", -1);



            Task.Run(async () =>
            {
                connectedUser = await _daoUsers.GetUserByIdAsync(userId);
                LoadCredentialsAsync();
            });
        }


        private async Task LoadCredentialsAsync()
        {
            var (credentialsList, error) = await _daoCredentials.GetAllCredentialsAsync();
            if (error != null)
            {
                await App.Current.MainPage.DisplayAlert("Error", error, "OK");
                return;
            }
            Credentials = new ObservableCollection<Credentials>(credentialsList);
        }

        public async void CopyPassword(int id)
        {
            var credential = _credentials.FirstOrDefault(c => c.Id == id);
            if (credential != null)
            {
                try
                {
                    string actualPassword = credential.GetActualPassword(connectedUser.EncryptionKey, connectedUser.EncryptionIV);
                    if (!string.IsNullOrEmpty(actualPassword))
                    {
                        await Clipboard.SetTextAsync(actualPassword);
                        await App.Current.MainPage.DisplayAlert("Success", "Password copied to clipboard!", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Password is empty", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }

        public async void DeletePassword(int id)
        {
            var userResponse = await App.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to delete this?", "Yes", "No");
            if (userResponse)
            {
                var message = await _daoCredentials.DeleteCredentialAsync(id);
                if (message == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", message, "OK");
                    return;
                }
                else
                {
                    var credentialToRemove = _credentials.FirstOrDefault(c => c.Id == id);
                    if (credentialToRemove != null)
                        Credentials.Remove(credentialToRemove);
                }
            }
        }
    }
}
