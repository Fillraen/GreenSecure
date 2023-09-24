using NT_GreenSecure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace NT_GreenSecure.Services
{
    public interface IDao_Credentials
    {
        Task<(ObservableCollection<Credentials> Result, string Error)> GetAllCredentialsAsync();
        Task<(Credentials Result, string Error)> GetCredentialByIdAsync(int id);
        Task<string> AddCredentialAsync(Credentials credential);
        Task<string> UpdateCredentialAsync(Credentials credential);
        Task<string> DeleteCredentialAsync(int id);
    }
}
