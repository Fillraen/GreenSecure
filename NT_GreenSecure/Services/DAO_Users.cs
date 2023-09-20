using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NT_GreenSecure.Models;
using System;
using System.Reflection;

namespace NT_GreenSecure.Services
{
    public class DAO_Users
    {
        public DAO_Users()
        {

        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("NT_GreenSecure.CustomData.users.json");

            using (StreamReader reader = new StreamReader(stream))
            {
                string json = await reader.ReadToEndAsync();
                List<UserJSON> usersJson = JsonConvert.DeserializeObject<List<UserJSON>>(json);

                List<User> users = new List<User>();
                foreach (var userJson in usersJson)
                {
                    User user = new User();
                    // Copier les autres champs ici.
                    user.UserId = userJson.UserId;
                    user.Username = userJson.Username;
                    user.Email = userJson.Email;
                    user.EncryptedPassword = userJson.EncryptedPassword;
                    user.CreatedDate = userJson.CreatedDate;
                    user.LastLoginDate = userJson.LastLoginDate;
                    user.IsLocked = userJson.IsLocked;
                    user.FailedLoginAttempts = userJson.FailedLoginAttempts;
                    user.UserCredentials = userJson.UserCredentials;
                    user.SetEncryptionKey(userJson.EncryptionKey);
                    user.SetEncryptionIV(userJson.EncryptionIV);

                    users.Add(user);
                }

                return users;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            List<User> users = await GetAllUsersAsync();
            User user = users.Find(u => u.Email == email);
            return user;
        }
    }
}