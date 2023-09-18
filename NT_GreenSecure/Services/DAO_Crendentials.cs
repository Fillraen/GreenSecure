using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NT_GreenSecure.Models;
using System.Reflection;
using Xamarin.Essentials;
using System.Linq;
using System;

namespace NT_GreenSecure.Services
{
    public class DAO_Credentials
    {
        public DAO_Credentials()
        {

        }
        public event EventHandler CredentialsChanged;

        public async Task<List<Credentials>> GetAllCredentialsAsync()
        {

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;

            Stream stream = assembly.GetManifestResourceStream("NT_GreenSecure.CustomData.credentials.json");

            using (StreamReader reader = new StreamReader(stream))
            {
                string json = await reader.ReadToEndAsync();
                List<Credentials> credentials = JsonConvert.DeserializeObject<List<Credentials>>(json);
                int IdUser = Preferences.Get("IdUser", int.MinValue);

                return credentials.Where(c => c.IdUser == IdUser).ToList();
            }
        }


        public async Task<Credentials> GetCredentialsByIdAsync(int id)
        {
            var credentials = await GetAllCredentialsAsync();
            return credentials.FirstOrDefault(c => c.Id == id);
        }

        public async Task AddCredentialAsync(Credentials newCredential)
        {
            List<Credentials> credentials = await GetAllCredentialsAsync();
            credentials.Add(newCredential);
            // Sauvegarder dans le fichier JSON
            SaveToFile(credentials);

            CredentialsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task UpdateCredentialAsync(Credentials updatedCredential)
        {
            List<Credentials> credentials = await GetAllCredentialsAsync();
            var existingCredential = credentials.FirstOrDefault(c => c.Id == updatedCredential.Id);
            if (existingCredential != null)
            {
                // Mettre à jour les champs
                existingCredential.Username = updatedCredential.Username;
                existingCredential.EmailAddress = updatedCredential.EmailAddress;
                existingCredential.Url = updatedCredential.Url;
                existingCredential.Name = updatedCredential.Name;
                // etc.

                // Sauvegarder dans le fichier JSON
                SaveToFile(credentials);

                CredentialsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task DeleteCredentialAsync(int id)
        {
            List<Credentials> credentials = await GetAllCredentialsAsync();
            var credentialToRemove = credentials.FirstOrDefault(c => c.Id == id);
            if (credentialToRemove != null)
            {
                credentials.Remove(credentialToRemove);
                // Sauvegarder dans le fichier JSON
                SaveToFile(credentials);

                CredentialsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SaveToFile(List<Credentials> credentials)
        {
            string json = JsonConvert.SerializeObject(credentials);
            
            // Écrire ICI le JSON dans le fichier
            
        }



    }
}