using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Konscious.Security.Cryptography;
using NT_GreenSecure.Services;

namespace NT_GreenSecure.Models
{
    public class Credentials : AesEncryption
    {
        // Propriétés pour stocker les informations de connexion
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string EncryptedPassword { get; set; }
        public string Domain { get; set; }
        public string Category { get; set; }
        public string DateCreated { get; set; }
        public string LastModified { get; set; }
        public int Complexity { get; set; }
        public string urlIcon { get; set; }


        public Credentials()
        {
        }

        // Méthode pour définir le mot de passe enregistré et calculer sa complexité
        public void SetPassword(string plainTextPassword, string EncryptionKey, string EncryptionIV)
        {
            EncryptedPassword = EncryptPassword(plainTextPassword, EncryptionKey, EncryptionIV);
            Complexity = EvaluatePasswordComplexity(plainTextPassword);
        }

        // Méthode pour obtenir le mot de passe en clair à partir de l'encryption
        public string GetActualPassword(string EncryptionKey, string EncryptionIV)
        {
            return DecryptPassword(EncryptedPassword, EncryptionKey, EncryptionIV);
        }


        public int EvaluatePasswordComplexity(string password)
        {
            int complexityScore = 0;

            // Has at least one lowercase letter
            if (Regex.IsMatch(password, "[a-z]"))
                complexityScore += 20;

            // Has at least one uppercase letter
            if (Regex.IsMatch(password, "[A-Z]"))
                complexityScore += 20;

            // Has at least one digit
            if (Regex.IsMatch(password, "[0-9]"))
                complexityScore += 20;

            // Has at least one special character
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
                complexityScore += 20;

            // Is of sufficient length
            if (password.Length >= 12)
                complexityScore += 20;

            return complexityScore;
        }

        // Méthode pour définir l'icône d'URL associée aux informations de connexion
        public void setUrlIcon(string url)
        {
            urlIcon = $"https://t0.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url={url}&size=256";
        }
    }
}
