// Interface IDao_Users<T> - Définit un contrat pour les opérations de gestion des utilisateurs
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NT_GreenSecure.Services
{
    public interface IDao_Users<T>
    {
        // Méthode pour obtenir tous les utilisateurs
        Task<(ObservableCollection<T> Result, string Error)> GetAllUsersAsync();

        // Méthode pour obtenir un utilisateur par son ID
        Task<(T Result, string Error)> GetUserByIdAsync(int id);

        // Méthode pour obtenir un utilisateur par son adresse e-mail
        Task<(T Result, string Error)> GetUserByEmailAsync(string email);

        // Méthode pour ajouter un utilisateur
        Task<string> AddUserAsync(T user);

        // Méthode pour mettre à jour un utilisateur
        Task<string> UpdateUserAsync(T user);

        // Méthode pour supprimer un utilisateur par son ID
        Task<string> DeleteUserAsync(int id);
    }
}