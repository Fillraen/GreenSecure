using NT_GreenSecure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
// ...
using Rg.Plugins.Popup.Services;
using NT_GreenSecure.ViewModels.Popup;
using NT_GreenSecure.Views.Popup;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NT_GreenSecure.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        // Commandes pour charger les données, ouvrir les détails du Credential, copier le mot de passe et supprimer le mot de passe
        public ICommand LoadDataCommand { get; set; }
        public ICommand OpenCredentialDetailCommand { get; set; }
        public ICommand CopyPasswordCommand { get; set; }
        public ICommand DeletePasswordCommand { get; set; }

        #region Properties

        // Propriété pour stocker l'utilisateur connecté
        private User _user;
        public User connectedUser
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        // Propriété pour stocker les nouveaux Credentials
        private ObservableCollection<Credentials> _newCredentials;
        public ObservableCollection<Credentials> NewCredentials
        {
            get => _newCredentials;
            set => SetProperty(ref _newCredentials, value);
        }

        // Propriété pour indiquer si le rafraîchissement est en cours
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        // Propriété pour stocker tous les Credentials
        private ObservableCollection<Credentials> _allCredentials;
        public ObservableCollection<Credentials> AllCredentials
        {
            get => _allCredentials;
            set => SetProperty(ref _allCredentials, value);
        }

        // Propriété pour stocker la complexité moyenne
        private int _averageComplexity;
        public int AverageComplexity
        {
            get => _averageComplexity;
            set => SetProperty(ref _averageComplexity, value);
        }

        // Propriété pour stocker le nombre total de mots de passe
        private int _totalPassword;
        public int TotalPassword
        {
            get => _totalPassword;
            set => SetProperty(ref _totalPassword, value);
        }

        // Propriété pour stocker le nombre de mots de passe réutilisés
        private int _reusedPassword;
        public int ReusedPassword
        {
            get => _reusedPassword;
            set => SetProperty(ref _reusedPassword, value);
        }

        // Dictionnaire pour suivre les comptes de mots de passe réutilisés
        private Dictionary<string, int> _passwordCounts = new Dictionary<string, int>();

        #endregion

        private int userId;

        public HomeViewModel()
        {
            Title = "Home"; // Titre de la vue

            userId = Preferences.Get("IdUser", -1);

            // Commandes pour copier et supprimer les mots de passe
            CopyPasswordCommand = new Command<int>(CopyPassword);
            DeletePasswordCommand = new Command<int>(DeletePassword);
            OpenCredentialDetailCommand = new Command<Credentials>(OpenCredentialDetail);
            LoadDataCommand = new Command(async () => await LoadData());

            // Récupération de l'utilisateur connecté
            Task.Run(async () =>
            {
                try
                {
                    var (user, error) = await DaoUsers.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        connectedUser = user;
                        await LoadData();
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

            // Abonnements aux messages de rafraîchissement des listes depuis les popups
            MessagingCenter.Subscribe<CredentialDetailViewModel>(this, "RefreshList", async (sender) =>
            {
                Task.Run(async () =>
                {
                    await LoadData();
                }).Wait();
            });

            MessagingCenter.Subscribe<AddPasswordViewModel>(this, "RefreshList", async (sender) =>
            {
                Task.Run(async () =>
                {
                    await LoadData();
                }).Wait();
            });
        }

        // Méthode pour charger les données
        private async Task LoadData()
        {
            IsRefreshing = true;
            await LoadCredentialsAsync();
            CalculateStatistics();
            IsRefreshing = false;
        }

        // Méthode pour charger les Credentials depuis la base de données
        private async Task LoadCredentialsAsync()
        {
            var (credentialsList, error) = await DaoCredentials.GetAllCredentialsAsync();
            if (error != null)
            {
                await App.Current.MainPage.DisplayAlert("Error", error, "OK");
                return;
            }
            AllCredentials = new ObservableCollection<Credentials>(credentialsList);
        }

        // Méthode pour calculer les statistiques (complexité moyenne, total de mots de passe, mots de passe réutilisés, etc.)
        private void CalculateStatistics()
        {
            // Calcul de la complexité moyenne
            if (AllCredentials.Count > 0)
            {
                AverageComplexity = AllCredentials.Sum(c => c.Complexity) / AllCredentials.Count;
            }

            // Calcul du nombre total de mots de passe
            TotalPassword = AllCredentials.Count;

            // Compter et suivre les mots de passe réutilisés
            _passwordCounts.Clear();
            foreach (var credential in AllCredentials)
            {
                if (!_passwordCounts.ContainsKey(credential.EncryptedPassword))
                {
                    _passwordCounts.Add(credential.EncryptedPassword, 1);
                }
                else
                {
                    _passwordCounts[credential.EncryptedPassword]++;
                }
            }

            // Trouver le nombre maximum (le mot de passe le plus réutilisé)
            int maxCount = 0;
            foreach (var count in _passwordCounts.Values)
            {
                if (count > maxCount)
                {
                    maxCount = count;
                }
            }

            // Définir ReusedPassword sur le nombre maximum
            ReusedPassword = maxCount;

            // Obtenir les 3 nouveaux Credentials
            NewCredentials = new ObservableCollection<Credentials>(AllCredentials
                .OrderByDescending(c => c.DateCreated)
                .Take(3));
        }

        // Méthode pour ouvrir les détails d'un Credential
        private async void OpenCredentialDetail(Credentials selectedCredential)
        {
            var page = new CredentialDetailPopup(selectedCredential);
            await PopupNavigation.Instance.PushAsync(page);
        }

        // Méthode pour copier un mot de passe
        public async void CopyPassword(int id)
        {
            var credential = _newCredentials.FirstOrDefault(c => c.Id == id);
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

        // Méthode pour supprimer un mot de passe
        public async void DeletePassword(int id)
        {
            var userResponse = await App.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to delete this?", "Yes", "No");
            if (userResponse)
            {
                var message = await DaoCredentials.DeleteCredentialAsync(id);
                if (message != "ok")
                {
                    await App.Current.MainPage.DisplayAlert("Error", message, "OK");
                    return;
                }
                else
                {
                    var credentialToRemove = AllCredentials.FirstOrDefault(c => c.Id == id);
                    if (credentialToRemove != null)
                    {
                        AllCredentials.Remove(credentialToRemove);
                        NewCredentials.Remove(credentialToRemove);
                        // Réinitialiser NewCredentials avec les trois derniers Credentials
                        CalculateStatistics();
                    }
                }
            }
        }
    }
}
