// Classe DAO_Users - Gère les opérations liées aux utilisateurs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NT_GreenSecure.Models;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace NT_GreenSecure.Services
{
    public class DAO_Users : IDao_Users<User>
    {
        private readonly string baseUrl = "http://10.0.2.2:8089/users"; // URL de base pour les opérations CRUD sur les utilisateurs
        private readonly HttpClient _client;

        public DAO_Users()
        {
            _client = new HttpClient(); 
        }

        // Méthode pour obtenir tous les utilisateurs
        public async Task<(ObservableCollection<User> Result, string Error)> GetAllUsersAsync()
        {
            Uri uri = new Uri(baseUrl);
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<ObservableCollection<User>>(content);

                    return (users, null); // Retourne la collection d'utilisateurs et aucune erreur
                }
                return (null, $"Error: {response.ReasonPhrase}"); // Retourne une erreur si la requête n'a pas réussi
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCredentialsAsync: {ex.Message}");
                return (null, $"Exception: {ex.Message}"); // Retourne une erreur en cas d'exception
            }
        }

        // Méthode pour obtenir un utilisateur par son ID
        public async Task<(User Result, string Error)> GetUserByIdAsync(int id)
        {
            Uri uri = new Uri($"{baseUrl}/{id}");
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(content);
                    return (user, null);
                }
                return (null, $"Error: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCredentialByIdAsync: {ex.Message}");
                return (null, $"Exception: {ex.Message}");
            }
        }

        // Méthode pour obtenir un utilisateur par son adresse e-mail
        public async Task<(User Result, string Error)> GetUserByEmailAsync(string email)
        {
            Uri uri = new Uri($"{baseUrl}/user/by-email");
            try
            {
                var request = new HttpRequestMessage();
                request.RequestUri = uri;
                request.Method = HttpMethod.Get;
                request.Headers.Add("Email", email);

                HttpResponseMessage response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(responseContent);
                    return (user, null);
                }
                return (null, $"Error: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByEmailAsync: {ex.Message}");
                return (null, $"Exception: {ex.Message}");
            }
        }

        // Méthode pour ajouter un utilisateur
        public async Task<string> AddUserAsync(User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _client.PostAsync(baseUrl, content);
                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.ReasonPhrase}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return null;
        }

        // Méthode pour mettre à jour un utilisateur
        public async Task<string> UpdateUserAsync(User user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _client.PutAsync($"{baseUrl}/{user.UserId}", content);
                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.ReasonPhrase}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return null;
        }

        // Méthode pour supprimer un utilisateur
        public async Task<string> DeleteUserAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"{baseUrl}/{id}");
                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.ReasonPhrase}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return null;
        }
    }
}
