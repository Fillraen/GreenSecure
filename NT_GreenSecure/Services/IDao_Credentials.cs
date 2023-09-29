using NT_GreenSecure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace NT_GreenSecure.Services
{
    public interface IDao_Credentials<T>
    {
        Task<(ObservableCollection<T> Result, string Error)> GetAllCredentialsAsync();
        Task<(T Result, string Error)> GetCredentialByIdAsync(int id);
        Task<string> AddCredentialAsync(T credential);
        Task<string> UpdateCredentialAsync(T credential);
        Task<string> DeleteCredentialAsync(int id);
    }
}
