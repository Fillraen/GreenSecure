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
        public ICommand CopyPasswordCommand { get; set; }
        public ICommand DeletePasswordCommand { get; set; }

        private ObservableCollection<Credentials> _credentials;
        public ObservableCollection<Credentials> Credentials
        {
            get => _credentials;
            set => SetProperty(ref _credentials, value);
        }

        private DAO_Credentials _daoCredentials;

        public VaultViewModel()
        {
            Title = "Vault";
            _daoCredentials = new DAO_Credentials();

            _daoCredentials.CredentialsChanged += async (sender, args) => await LoadCredentialsAsync();
            LoadCredentialsAsync().Wait();

            CopyPasswordCommand = new Command<int>(CopyPassword);
            DeletePasswordCommand = new Command<int>(DeletePassword);
        }

        private async Task LoadCredentialsAsync()
        {
            var credentialsList = await _daoCredentials.GetAllCredentialsAsync();
            Credentials = new ObservableCollection<Credentials>(credentialsList);
        }

        private void CopyPassword(int id)
        {
            // Trouvez le mot de passe correspondant à l'ID
            var credential = _credentials.FirstOrDefault(c => c.Id == id);
            if (credential != null)
            {
                // string actualPassword = credential.GetActualPassword();
                string actualPassword = "test";
                if (!string.IsNullOrEmpty(actualPassword))
                {
                    Clipboard.SetTextAsync(actualPassword);
                }
            }
        }


        private async void DeletePassword(int id)
        {
            // Supprimez le mot de passe en utilisant DAO_Credentials
            await _daoCredentials.DeleteCredentialAsync(id);
        }
    }
}
