using NT_GreenSecure.Models;
using NT_GreenSecure.Services;
using NT_GreenSecure.ViewModels.Popup;
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
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    // Appel à une méthode pour filtrer les résultats ici
                    FilterCredentials();
                }
            }
        }

        private ObservableCollection<Credentials> _allCredentials;

        public ObservableCollection<Credentials> AllCredentials
        {
            get => _allCredentials;
            set => SetProperty(ref _allCredentials, value);
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set => SetProperty(ref _selectedFilter, value);
        }

        private List<string> _filterOptions = new List<string> { "All", "Site Web", "Application" };
        public List<string> FilterOptions => _filterOptions;
        public ICommand FilterCommand => new Command<string>((filter) =>
        {
            SelectedFilter = filter;
            FilterCredentials();
        });

        public ICommand LoadDataCommand { get; }
        private int userId;
        private User connectedUser;

        public VaultViewModel()
        {
            Title = "Vault";
            userId = Preferences.Get("IdUser", -1);

            CopyPasswordCommand = new Command<int>(CopyPassword);
            DeletePasswordCommand = new Command<int>(DeletePassword);
            OpenCredentialDetailCommand = new Command<Credentials>(OpenCredentialDetail);
            LoadDataCommand = new Command(async () => await LoadData());
            SelectedFilter = "All"; // Initialize the selected filter
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
        }

        private async Task LoadData()
        {
            IsRefreshing = true;
            await LoadCredentialsAsync(); // Charge toutes les données
            FilterCredentials(); // Filtre les données en fonction de SearchText
            IsRefreshing = false;
        }


        private void FilterCredentials()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // If the search text is empty, filter based on the selected filter
                if (SelectedFilter == "All")
                {
                    Credentials = new ObservableCollection<Credentials>(AllCredentials);
                }
                else
                {
                    Credentials = new ObservableCollection<Credentials>(
                        AllCredentials.Where(c => c.Domain == SelectedFilter)
                    );
                }
            }
            else
            {
                // Filter credentials based on the search text and selected filter
                Credentials = new ObservableCollection<Credentials>(
                    AllCredentials.Where(c =>
                        (c.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) &&
                        (SelectedFilter == "All" || c.Domain == SelectedFilter)
                    )
                );
            }
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
            AllCredentials = new ObservableCollection<Credentials>(credentialsList);
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
                if (message != "ok")
                {
                    await App.Current.MainPage.DisplayAlert("Error", message, "OK");
                    return;
                }
                else
                {
                    var credentialToRemove = _credentials.FirstOrDefault(c => c.Id == id);
                    if (credentialToRemove != null)
                    {
                        Credentials.Remove(credentialToRemove);
                    }
                }
            }
        }
    }
}
