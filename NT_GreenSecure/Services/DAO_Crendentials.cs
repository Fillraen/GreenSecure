using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NT_GreenSecure.Models;
using System.Reflection;
using Xamarin.Essentials;
using System.Linq;

namespace NT_GreenSecure.Services
{
    public class DAO_Credentials
    {
        public DAO_Credentials()
        {

        }

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
    }
}