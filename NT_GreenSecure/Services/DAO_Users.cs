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
                List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                return users;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            List<User> users = await GetAllUsersAsync();
            User user = users.Find(u => u.Email == email);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id){
            List<User> users = await GetAllUsersAsync();
            User user = users.Find(u => u.UserId == id);
            return user;
        }   
    }

}