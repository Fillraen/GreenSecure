using NT_GreenSecure.Models;
using NT_GreenSecure.Services;
using NT_GreenSecure.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace NT_GreenSecure.ViewModels
{
    public class VaultViewModel : BaseViewModel
    {
        public ICommand OpenCredentialDetailCommand { get; set; }
        public ICommand CopyPasswordCommand { get; set; }
        public ICommand DeletePasswordCommand { get; set; }

        private ObservableCollection<Credentials> _credentials;
        public ObservableCollection<Credentials> Credentials
        {
            get => _credentials;
            set => SetProperty(ref _credentials, value);
        }

        private int userId;
        private User connectedUser;

        public VaultViewModel()
        {
            Title = "Vault";
            userId = Preferences.Get("IdUser", -1);

            CopyPasswordCommand = new Command<int>(CopyPassword);
            DeletePasswordCommand = new Command<int>(DeletePassword);
            OpenCredentialDetailCommand = new Command<Credentials>(OpenCredentialDetail);

            Task.Run(async () =>
            {
                try
                {
                    var (user, error) = await DaoUsers.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        connectedUser = user;
                        LoadCredentialsAsync();
                    }
                    else
                    {
                        // Gérez l'erreur ici si nécessaire, par exemple en affichant un message à l'utilisateur.
                        Debug.WriteLine($"Error loading user: {error}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading user: {ex.Message}");
                }
                
            });

        }

        private async void OpenCredentialDetail(Credentials selectedCredential)
        {
            var page = new CredentialDetailPopup(selectedCredential);
            await PopupNavigation.Instance.PushAsync(page); 
        }

        private async Task LoadCredentialsAsync()
        {
            var (credentialsList, error) = await DaoCredentials.GetAllCredentialsAsync();
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
                var message = await DaoCredentials.DeleteCredentialAsync(id);
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
