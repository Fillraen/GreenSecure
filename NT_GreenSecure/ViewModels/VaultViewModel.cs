using NT_GreenSecure.Models;
using NT_GreenSecure.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

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

        private DAO_Credentials _daoCredentials;

        public VaultViewModel()
        {
            Title = "Vault";
            _daoCredentials = new DAO_Credentials();
            LoadCredentialsAsync();
        }

        private async Task LoadCredentialsAsync()
        {
            var credentialsList = await _daoCredentials.GetAllCredentialsAsync();
            Credentials = new ObservableCollection<Credentials>(credentialsList);
        }
    }
}
