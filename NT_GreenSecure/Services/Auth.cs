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
        private AesEncryption _aesEncryption;

        public Auth()
        {
            _daoUser = new DAO_Users();
            _aesEncryption = new AesEncryption();
        }

        public async Task<bool> AuthenticateAsync(string email, string plainTextPassword)
        {
            List<User> users = await _daoUser.GetAllUsersAsync();
            User user = users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return false;
            }

            // bool isAuthenticated = userCredentials.ValidatePassword(plainTextPassword);
            bool isAuthenticated = _aesEncryption.VerifyPassword(user.EncryptedPassword, plainTextPassword, user.EncryptionKey, user.EncryptionIV);

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