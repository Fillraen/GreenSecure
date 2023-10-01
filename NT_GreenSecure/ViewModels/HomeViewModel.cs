using NT_GreenSecure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NT_GreenSecure.ViewModels.Popup;
using Xamarin.Essentials;
using Xamarin.Forms;
using NT_GreenSecure.Views.Popup;
using Rg.Plugins.Popup.Services;
using System.Net;

namespace NT_GreenSecure.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand LoadDataCommand { get; set; }
        public ICommand OpenCredentialDetailCommand { get; set; }
        public ICommand CopyPasswordCommand { get; set; }
        public ICommand DeletePasswordCommand { get; set; }

        #region Properties

        private User _user;
        public User connectedUser
        {
           get => _user;
           set => SetProperty(ref _user, value);
        }

        private ObservableCollection<Credentials> _newCredentials;
        public ObservableCollection<Credentials> NewCredentials
        {
            get => _newCredentials;
            set => SetProperty(ref _newCredentials, value);
        }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private ObservableCollection<Credentials> _allCredentials;

        public ObservableCollection<Credentials> AllCredentials
        {
            get => _allCredentials;
            set => SetProperty(ref _allCredentials, value);
        }

        private int _averageComplexity;
        public int AverageComplexity
        {
            get => _averageComplexity;
            set => SetProperty(ref _averageComplexity, value);
        }
        private int _totalPassword;
        public int TotalPassword
        {
            get => _totalPassword;
            set => SetProperty(ref _totalPassword, value);
        }
        private int _reusedPassword;
        public int ReusedPassword
        {
            get => _reusedPassword;
            set => SetProperty(ref _reusedPassword, value);
        }

        private Dictionary<string, int> _passwordCounts = new Dictionary<string, int>();

        #endregion

        private int userId;

        public HomeViewModel()
        {
            Title = "Home";
            userId = Preferences.Get("IdUser", -1);
            CopyPasswordCommand = new Command<int>(CopyPassword);
            DeletePasswordCommand = new Command<int>(DeletePassword);
            OpenCredentialDetailCommand = new Command<Credentials>(OpenCredentialDetail);
            LoadDataCommand = new Command(async () => await LoadData());

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

        private async Task LoadData()
        {
            IsRefreshing = true;
            await LoadCredentialsAsync();
            CalculateStatistics();
            IsRefreshing = false;
        }

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

        private void CalculateStatistics()
        {
            // Calculate AverageComplexity
            if (AllCredentials.Count > 0)
            {
                AverageComplexity = AllCredentials.Sum(c => c.Complexity) / AllCredentials.Count;
            }

            // Calculate TotalPassword
            TotalPassword = AllCredentials.Count;

            // Count and track reused passwords
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

            // Find the largest count (most reused password)
            int maxCount = 0;
            foreach (var count in _passwordCounts.Values)
            {
                if (count > maxCount)
                {
                    maxCount = count;
                }
            }

            // Set ReusedPassword to the largest count
            ReusedPassword = maxCount;

            // Get the 3 newest credentials
            NewCredentials = new ObservableCollection<Credentials>(AllCredentials
                .OrderByDescending(c => c.DateCreated)
                .Take(3));
        }

        private async void OpenCredentialDetail(Credentials selectedCredential)
        {
            var page = new CredentialDetailPopup(selectedCredential);
            await PopupNavigation.Instance.PushAsync(page);
        }

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
                        // Reinitialize NewCredentials with the three newest credentials
                        CalculateStatistics();
                    }
                }
            }
        }

    }
}
