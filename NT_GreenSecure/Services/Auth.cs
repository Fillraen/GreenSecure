using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NT_GreenSecure.Models;
using NT_GreenSecure.Services;
using Xamarin.Essentials;

namespace NT_GreenSecure
{
    public class Auth : AesEncryption
    {
        private DAO_Users _daoUser;

        public Auth()
        {
            _daoUser = new DAO_Users();
        }

        public async Task<bool> AuthenticateAsync(string email, string plainTextPassword)
        {
            User user = await _daoUser.GetUserByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            // bool isAuthenticated = userCredentials.ValidatePassword(plainTextPassword);
            bool isAuthenticated = VerifyPassword(user.EncryptedPassword, plainTextPassword, user.GetEncryptionKey(), user.GetEncryptionIV());

            if (isAuthenticated)
            {
                user.LastLoginDate = DateTime.UtcNow;
                user.FailedLoginAttempts = 0;
                
                Preferences.Set("IdUser", user.UserId);
                Preferences.Set("token_expiry", DateTime.UtcNow.AddHours(48).Ticks);

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

    }
}