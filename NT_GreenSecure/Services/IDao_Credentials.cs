// Interface IDao_Credentials<T> - Définit un contrat pour les opérations de gestion des informations d'identification (credentials)
using NT_GreenSecure.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NT_GreenSecure.Services
{
    public interface IDao_Credentials<T>
    {
        // Méthode pour obtenir toutes les informations d'identification
        Task<(ObservableCollection<T> Result, string Error)> GetAllCredentialsAsync();

        // Méthode pour obtenir une information d'identification par son ID
        Task<(T Result, string Error)> GetCredentialByIdAsync(int id);

        // Méthode pour ajouter une information d'identification
        Task<string> AddCredentialAsync(T credential);

        // Méthode pour mettre à jour une information d'identification
        Task<string> UpdateCredentialAsync(T credential);

        // Méthode pour supprimer une information d'identification par son ID
        Task<string> DeleteCredentialAsync(int id);
    }
}