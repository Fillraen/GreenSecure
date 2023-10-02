// Classe DAO_Credentials - Gère les opérations liées aux informations d'identification (credentials)
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NT_GreenSecure.Models;
using System.Reflection;
using Xamarin.Essentials;
using System.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace NT_GreenSecure.Services
{
    public class DAO_Credentials : IDao_Credentials<Credentials>
    {
        private readonly string baseUrl = "http://nicolas-trehou.alwaysdata.net/credentials"; // URL de base pour les opérations CRUD sur les credentials
        private readonly HttpClient _client; 
        private int userId; 

        public DAO_Credentials()
        {
            _client = new HttpClient();
            // Récupération de l'ID de l'utilisateur à partir des préférences
            userId = Preferences.Get("IdUser", -1);
        }

        // Méthode pour obtenir toutes les informations d'identification de l'utilisateur actuel
        public async Task<(ObservableCollection<Credentials> Result, string Error)> GetAllCredentialsAsync()
        {
            Uri uri = new Uri($"{baseUrl}/user/{userId}"); // URL pour obtenir les credentials de l'utilisateur
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var credentials = JsonConvert.DeserializeObject<ObservableCollection<Credentials>>(content);

                    // Parcourt chaque Credentials dans la collection pour appeler setUrlIcon
                    foreach (var credential in credentials)
                    {
                        credential.setUrlIcon(credential.Url); // Appel de la méthode setUrlIcon pour chaque Credentials
                    }

                    return (credentials, null); // Retourne la collection de credentials et aucune erreur
                }
                return (null, $"Error: {response.ReasonPhrase}"); // Retourne une erreur si la requête n'a pas réussi
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCredentialsAsync: {ex.Message}");
                return (null, $"Exception: {ex.Message}"); // Retourne une erreur en cas d'exception
            }
        }

        // Méthode pour obtenir un credential par son ID

        public async Task<(Credentials Result, string Error)> GetCredentialByIdAsync(int id)
        {
            Uri uri = new Uri($"{baseUrl}/{id}");
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var credential = JsonConvert.DeserializeObject<Credentials>(content);
                    return (credential, null);
                }
                return (null, $"Error: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCredentialByIdAsync: {ex.Message}");
                return (null, $"Exception: {ex.Message}");
            }
        }

        // Méthode pour ajouter un credential

        public async Task<string> AddCredentialAsync(Credentials credential)
        {
            var content = new StringContent(JsonConvert.SerializeObject(credential), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _client.PostAsync(baseUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    return $"Error: {response.ReasonPhrase}";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return null;
        }

        // Méthode pour mettre à jour un credential

        public async Task<string> UpdateCredentialAsync(Credentials credential)
        {
            var content = new StringContent(JsonConvert.SerializeObject(credential), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _client.PutAsync($"{baseUrl}/{credential.Id}", content);
                if (!response.IsSuccessStatusCode)
                {
                    return $"Error: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return null;
        }

        // Méthode pour supprimer un credential

        public async Task<string> DeleteCredentialAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"{baseUrl}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return $"Error: {response.ReasonPhrase}";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return "ok";
        }
    }
}
