using NT_GreenSecure.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NT_GreenSecure.ViewModels.Popup
{
    public class AddPasswordViewModel : BaseViewModel
    {

        #region Properties

        public bool IsWebsiteSelected
        {
            get => NewCredential.Domain == "Site Web";
            set
            {
                if (value)
                {
                    NewCredential.Domain = "Site Web";
                }
                else
                {
                    NewCredential.Domain = "Application";
                }
                OnPropertyChanged(nameof(IsWebsiteSelected));
                OnPropertyChanged(nameof(IsAppSelected)); // À noter que IsAppSelected est le complément de IsWebsiteSelected
            }
        }

        public bool IsAppSelected
        {
            get => !IsWebsiteSelected; // C'est le complément logique de IsWebsiteSelected
        }


        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }

        private string _decryptedPassword;
        public string DecryptedPassword
        {
            get => _decryptedPassword;
            set
            {
                if (SetProperty(ref _decryptedPassword, value))
                {
                    NewCredential.SetPassword(value, connectedUser.EncryptionKey, connectedUser.EncryptionIV);
                    Complexity = NewCredential.Complexity; // Update ViewModel Property
                    OnPropertyChanged(nameof(PasswordStrength));
                }

            }
        }
        // Propriété pour la force du mot de passe
        public string PasswordStrength
        {
            get
            {
                if (NewCredential.Complexity < 34) return "Faible";
                if (NewCredential.Complexity < 67) return "Moyen";
                return "Fort";
            }
        }
        public int Complexity
        {
            get => NewCredential.Complexity;
            set
            {
                NewCredential.Complexity = value;
                OnPropertyChanged(nameof(Complexity)); // Notify Change
            }
        }
        #endregion

        private int userId;
        private User connectedUser;

        public ICommand TogglePasswordCommand { get; set; }

        public Credentials NewCredential { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddPasswordViewModel()
        {
            NewCredential = new Credentials();

            userId = Preferences.Get("IdUser", -1);
            NewCredential.IdUser = userId;
            Task.Run(async () =>
            {
                try
                {
                    var (user, error) = await DaoUsers.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        connectedUser = user;
                        DecryptedPassword = NewCredential.GetActualPassword(connectedUser.EncryptionKey, connectedUser.EncryptionIV);
                    }
                    else
                    {
                        Debug.WriteLine($"Error loading user: {error}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading user: {ex.Message}");
                }
            }).Wait();

            IsWebsiteSelected = NewCredential.Domain == "Site Web";
            TogglePasswordCommand = new Command(TogglePassword);
            IsPasswordVisible = false;
            SaveCommand = new Command(async () => await SaveNewCredential());
            CancelCommand = new Command(async () => await CancelAddCredential());
            
        }

        private void TogglePassword()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

            private async Task SaveNewCredential()
            {
                try
                {
                   var error = await DaoCredentials.AddCredentialAsync(NewCredential);
                   if (string.IsNullOrEmpty(error))
                   {
                        // Fermer la popup après une mise à jour réussie
                        MessagingCenter.Send(this, "RefreshList");
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                   }
                   else
                   {
                       // Afficher l'erreur retournée à l'utilisateur
                       await Application.Current.MainPage.DisplayAlert("Erreur", $"Erreur durant l'ajout : {error}", "OK");
                   }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving new credential: {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Exception", $"Une exception s'est produite : {ex.Message}", "OK");
                }
            }

        private async Task CancelAddCredential()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
