using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Konscious.Security.Cryptography;

namespace NT_GreenSecure.Models
{
    public class Credentials
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; } // Email address associated with the credential
        public string Url { get; set; } // URL of the website or app
        public string Name { get; set; }
        private string EncryptedPassword { get; set; } // Encrypted Password
        public string Domain { get; set; } // Website or App the credential is for
        public string Category { get; set; } // E.g., Social Media, Banking
        public DateTime DateCreated { get; set; } // When was this credential created
        public DateTime LastModified { get; set; } // When was this credential last modified

        // Password Complexity and hashing
        public int Complexity { get; set; }
        public string Salt { get; set; }

        private Argon2Hasher _argon2Hasher = new Argon2Hasher();


        public void SetPassword(string plainTextPassword)
        {
            byte[] salt;
            EncryptedPassword = _argon2Hasher.HashPassword(plainTextPassword, out salt);
            Salt = Convert.ToBase64String(salt);  // Conversion en string
            EvaluatePasswordComplexity(plainTextPassword);
        }

        public bool ValidatePassword(string plainTextPassword)
        {
            byte[] saltBytes = Convert.FromBase64String(Salt);  // Conversion en byte[]
            return _argon2Hasher.VerifyPassword(EncryptedPassword, plainTextPassword, saltBytes);
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

        public string GetActualPassword()
        {
            if (string.IsNullOrEmpty(EncryptedPassword) || string.IsNullOrEmpty(Salt))
            {
                return null;
            }

            // Supposons que votre _argon2Hasher a une méthode pour décrypter
            byte[] saltBytes = Convert.FromBase64String(Salt);
            return _argon2Hasher.DecryptPassword(EncryptedPassword, saltBytes);
        }


    }
}
