using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NT_GreenSecure.Models;
using NT_GreenSecure.Services;
using Xamarin.Essentials;

namespace NT_GreenSecure
{
    public class Auth
    {
        private DAO_Users _daoUser;
        private Argon2Hasher _argon2Hasher;

        public Auth()
        {
            _daoUser = new DAO_Users();
            _argon2Hasher = new Argon2Hasher();
        }

        public async Task<bool> AuthenticateAsync(string email, string plainTextPassword)
        {
            List<User> users = await _daoUser.GetAllUsersAsync();
            User user = users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return false;
            }

            bool isAuthenticated = _argon2Hasher.VerifyPassword(user.EncryptedPassword, plainTextPassword, Convert.FromBase64String(user.Salt));

            if (isAuthenticated)
            {
                user.LastLoginDate = DateTime.UtcNow;
                user.FailedLoginAttempts = 0;
                Preferences.Set("IdUser", user.UserId);
                return true;
            }
            else
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 3)
                {
                    user.IsLocked = true;
                }
                return false;
            }
        }

        public string GenerateAccessToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}