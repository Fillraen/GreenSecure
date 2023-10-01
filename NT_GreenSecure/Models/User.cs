// Classe User - Représente un utilisateur de l'application
using Konscious.Security.Cryptography;
using NT_GreenSecure.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NT_GreenSecure.Models
{
    public class User : AesEncryption
    {
        // Propriétés pour stocker les informations de l'utilisateur
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string EncryptedPassword { get; set; }
        public string CreatedDate { get; set; }
        public string LastLoginDate { get; set; }
        public int IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }
        public List<Credentials> UserCredentials { get; set; }
        public string EncryptionKey { get; set; }
        public string EncryptionIV { get; set; }

        public User()
        {
            // Initialise les valeurs par défaut lors de la création d'un utilisateur
            CreatedDate = DateTime.UtcNow.ToString();
            UserCredentials = new List<Credentials>();
            IsLocked = 0;
            FailedLoginAttempts = 0;
        }

        // Méthode pour définir la clé d'encryption
        public void SetEncryptionKey(string key = null)
        {
            if (key == null)
            {
                using (Aes aes = Aes.Create())
                {
                    EncryptionKey = Convert.ToBase64String(aes.Key);
                }
            }
            else
            {
                EncryptionKey = key;
            }
        }

        // Méthode pour définir le vecteur d'initialisation d'encryption
        public void SetEncryptionIV(string iv = null)
        {
            if (iv == null)
            {
                using (Aes aes = Aes.Create())
                {
                    EncryptionIV = Convert.ToBase64String(aes.IV);
                }
            }
            else
            {
                EncryptionIV = iv;
            }
        }

        // Méthode pour obtenir la clé d'encryption
        public string GetEncryptionKey()
        {
            return EncryptionKey;
        }

        // Méthode pour obtenir le vecteur d'initialisation d'encryption
        public string GetEncryptionIV()
        {
            return EncryptionIV;
        }

        // Méthode pour définir le mot de passe encrypté
        public void SetEncryptedPassword(string plainTextPassword)
        {
            EncryptedPassword = EncryptPassword(plainTextPassword, EncryptionKey, EncryptionIV);
        }

        // Méthode pour obtenir le mot de passe en clair à partir de l'encryption
        public string GetPlainTextPassword()
        {
            return DecryptPassword(EncryptedPassword, EncryptionKey, EncryptionIV);
        }
    }
}
