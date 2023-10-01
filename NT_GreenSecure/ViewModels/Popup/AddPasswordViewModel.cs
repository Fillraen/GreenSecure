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

        // Propriété permettant de déterminer si le domaine du mot de passe est un site web
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

        // Propriété permettant de déterminer si le domaine du mot de passe est une application
        public bool IsAppSelected
        {
            get => !IsWebsiteSelected; // C'est le complément logique de IsWebsiteSelected
        }

        // Propriété pour gérer la visibilité du mot de passe en clair
        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }

        // Propriété pour le mot de passe en clair
        private string _decryptedPassword;
        public string DecryptedPassword
        {
            get => _decryptedPassword;
            set
            {
                if (SetProperty(ref _decryptedPassword, value))
                {
                    NewCredential.SetPassword(value, connectedUser.EncryptionKey, connectedUser.EncryptionIV);
                    Complexity = NewCredential.Complexity; // Met à jour la propriété ViewModel
                    OnPropertyChanged(nameof(PasswordStrength));
                }

            }
        }

        // Propriété pour évaluer la force du mot de passe
        public string PasswordStrength
        {
            get
            {
                if (NewCredential.Complexity < 34) return "Faible";
                if (NewCredential.Complexity < 67) return "Moyen";
                return "Fort";
            }
        }

        // Propriété pour la complexité du mot de passe
        public int Complexity
        {
            get => NewCredential.Complexity;
            set
            {
                NewCredential.Complexity = value;
                OnPropertyChanged(nameof(Complexity)); // Notifie le changement
            }
        }

        #endregion

        private int userId;
        private User connectedUser;

        // Commande pour basculer la visibilité du mot de passe
        public ICommand TogglePasswordCommand { get; set; }

        // Objet pour stocker les informations du nouveau mot de passe
        public Credentials NewCredential { get; set; }

        // Commande pour sauvegarder le nouveau mot de passe
        public ICommand SaveCommand { get; set; }

        // Commande pour annuler l'ajout du mot de passe
        public ICommand CancelCommand { get; set; }

        public AddPasswordViewModel()
        {
            NewCredential = new Credentials();

            userId = Preferences.Get("IdUser", -1);
            NewCredential.IdUser = userId;

            // Exécute une tâche asynchrone pour charger les informations de l'utilisateur connecté
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

            // Commande pour basculer la visibilité du mot de passe
            TogglePasswordCommand = new Command(TogglePassword);

            // Initialisation de la visibilité du mot de passe à faux
            IsPasswordVisible = false;

            // Commande pour sauvegarder le nouveau mot de passe
            SaveCommand = new Command(async () => await SaveNewCredential());

            // Commande pour annuler l'ajout du mot de passe
            CancelCommand = new Command(async () => await CancelAddCredential());

        }

        // Méthode pour basculer la visibilité du mot de passe
        private void TogglePassword()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        // Méthode pour sauvegarder le nouveau mot de passe
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

        // Méthode pour annuler l'ajout du mot de passe
        private async Task CancelAddCredential()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
