﻿using NT_GreenSecure.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NT_GreenSecure.ViewModels.Popup
{
    public class CredentialDetailViewModel : BaseViewModel
    {
        // Propriété pour stocker le Credential sélectionné
        public Credentials SelectedCredential { get; set; }

        // Commandes pour les boutons de sauvegarde, suppression et basculement de mot de passe
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand TogglePasswordCommand { get; set; }

        private int userId;
        private User connectedUser;

        #region Properties

        // Propriété pour vérifier si "Site Web" est sélectionné
        public bool IsWebsiteSelected
        {
            get => SelectedCredential.Domain == "Site Web";
            set
            {
                if (value)
                {
                    SelectedCredential.Domain = "Site Web";
                }
                else
                {
                    SelectedCredential.Domain = "Application";
                }
                OnPropertyChanged(nameof(IsWebsiteSelected));
                OnPropertyChanged(nameof(IsAppSelected)); // À noter que IsAppSelected est le complément de IsWebsiteSelected
            }
        }

        // Propriété pour vérifier si "Application" est sélectionné (complément de "Site Web")
        public bool IsAppSelected
        {
            get => !IsWebsiteSelected; // C'est le complément logique de IsWebsiteSelected
        }

        // Propriété pour gérer la visibilité du mot de passe
        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }

        // Propriété pour stocker le mot de passe déchiffré
        private string _decryptedPassword;
        public string DecryptedPassword
        {
            get => _decryptedPassword;
            set
            {
                if (SetProperty(ref _decryptedPassword, value))
                {
                    // Mettre à jour le mot de passe chiffré dans le Credential
                    SelectedCredential.SetPassword(value, connectedUser.EncryptionKey, connectedUser.EncryptionIV);
                    Complexity = SelectedCredential.Complexity; // Mettre à jour la propriété de complexité
                    OnPropertyChanged(nameof(PasswordStrength));
                }
            }
        }

        // Propriété pour afficher la force du mot de passe
        public string PasswordStrength
        {
            get
            {
                if (SelectedCredential.Complexity < 34) return "Faible";
                if (SelectedCredential.Complexity < 67) return "Moyen";
                return "Fort";
            }
        }

        // Propriété pour stocker la complexité du mot de passe
        public int Complexity
        {
            get => SelectedCredential.Complexity;
            set
            {
                SelectedCredential.Complexity = value;
                OnPropertyChanged(nameof(Complexity)); // Notifier le changement
            }
        }
        #endregion

        // Constructeur du ViewModel
        public CredentialDetailViewModel(Credentials selectedCredential)
        {
            Title = "Credential Detail"; // Titre de la vue

            userId = Preferences.Get("IdUser", -1);

            // Récupération de l'utilisateur connecté
            Task.Run(async () =>
            {
                try
                {
                    var (user, error) = await DaoUsers.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        connectedUser = user;
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

            SelectedCredential = selectedCredential;
            IsWebsiteSelected = SelectedCredential.Domain == "Site Web";
            TogglePasswordCommand = new Command(TogglePassword);
            SaveCommand = new Command(async () => await SaveCredential());
            DeleteCommand = new Command(async () => await DeleteCredentials());
            IsPasswordVisible = false; // Par défaut, le mot de passe est caché
            DecryptedPassword = selectedCredential.GetActualPassword(connectedUser.EncryptionKey, connectedUser.EncryptionIV);
        }

        // Méthode pour basculer la visibilité du mot de passe
        private void TogglePassword()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        // Méthode pour sauvegarder les modifications du Credential
        private async Task SaveCredential()
        {
            try
            {
                var error = await DaoCredentials.UpdateCredentialAsync(SelectedCredential);

                if (string.IsNullOrEmpty(error))
                {
                    // Fermer la popup après une mise à jour réussie
                    MessagingCenter.Send(this, "RefreshList");
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    // Afficher l'erreur retournée à l'utilisateur
                    await Application.Current.MainPage.DisplayAlert("Erreur", $"Erreur de sauvegarde : {error}", "OK");
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions, par exemple afficher une erreur à l'utilisateur
                Debug.WriteLine($"Error saving credential: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Exception", $"Une exception s'est produite : {ex.Message}", "OK");
            }
        }

        // Méthode pour supprimer le Credential
        private async Task DeleteCredentials()
        {
            try
            {
                var userResponse = await App.Current.MainPage.DisplayAlert("Confirmation", "Êtes-vous sûr de vouloir supprimer ceci?", "Oui", "Non");
                if (userResponse)
                {
                    var message = await DaoCredentials.DeleteCredentialAsync(SelectedCredential.Id);
                    if (message != "ok")
                    {
                        await App.Current.MainPage.DisplayAlert("Erreur", message, "OK");
                        return;
                    }
                    else
                    {
                        MessagingCenter.Send(this, "RefreshList");
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting credential: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Exception", $"Une exception s'est produite : {ex.Message}", "OK");
            }
        }
    }
}
