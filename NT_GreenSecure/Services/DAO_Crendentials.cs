using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NT_GreenSecure.Models;

namespace NT_GreenSecure.Services
{
    public class DAO_Credentials
    {
        public DAO_Credentials()
        {

        }

        public async Task<List<Credentials>> GetAllCredentialsAsync()
        {
            using (StreamReader r = new StreamReader("credentials.json"))
            {
                string json = await r.ReadToEndAsync();
                List<Credentials> credentials = JsonConvert.DeserializeObject<List<Credentials>>(json);
                return credentials;
            }
        }
    }
}