// Classe Auth - Gère l'authentification des utilisateurs
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

        // Méthode d'authentification asynchrone
        public async Task<bool> AuthenticateAsync(string email, string plainTextPassword)
        {
            // Récupère l'utilisateur associé à l'adresse e-mail spécifiée
            var (user, error) = await _daoUser.GetUserByEmailAsync(email);

            if (user == null)
            {
                // L'utilisateur n'existe pas, retourne false
                return false;
            }

            // Vérifie si le mot de passe spécifié correspond au mot de passe stocké
            bool isAuthenticated = VerifyPassword(user.EncryptedPassword, plainTextPassword, user.GetEncryptionKey(), user.GetEncryptionIV());

            if (isAuthenticated)
            {
                // Met à jour les informations de l'utilisateur en cas de succès de l'authentification
                user.LastLoginDate = DateTime.UtcNow.ToString();
                user.FailedLoginAttempts = 0;

                // Stocke l'identifiant de l'utilisateur et la date d'expiration du token dans les préférences
                Preferences.Set("IdUser", user.UserId);
                Preferences.Set("token_expiry", DateTime.UtcNow.AddHours(48).Ticks);

                return true;
            }
            else
            {
                // Incrémente le nombre de tentatives de connexion infructueuses
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 3)
                {
                    // Bloque l'utilisateur si le nombre de tentatives infructueuses dépasse 3
                    user.IsLocked = 1;
                }

                return false;
            }
        }
    }
}
