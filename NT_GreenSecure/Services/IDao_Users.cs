using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace NT_GreenSecure.Services
{
    public interface IDao_Users<T>
    {
        Task<(ObservableCollection<T> Result, string Error)> GetAllUsersAsync();
        Task<(T Result, string Error)> GetUserByIdAsync(int id);
        Task<(T Result, string Error)> GetUserByEmailAsync(string email);
        Task<string> AddUserAsync(T user);
        Task<string> UpdateUserAsync(T user);
        Task<string> DeleteUserAsync(int id);

    }
}
