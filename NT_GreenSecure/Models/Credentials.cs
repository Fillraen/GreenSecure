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
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; } // Email address associated with the credential
        public string Url { get; set; } // URL of the website or app
        public string Name { get; set; }
        public string EncryptedPassword { get; set; } // Encrypted Password
        
        public string Domain { get; set; } // Website or App the credential is for
        public string Category { get; set; } // E.g., Social Media, Banking
        public string DateCreated { get; set; } // When was this credential created
        public string LastModified { get; set; } // When was this credential last modified
        public int Complexity { get; set; }

        public string urlIcon { get; set; }

        public Credentials (){
        }

        public void SetPassword(string plainTextPassword, string EncryptionKey, string EncryptionIV)
        {
            EncryptedPassword = EncryptPassword(plainTextPassword, EncryptionKey, EncryptionIV);
            EvaluatePasswordComplexity(plainTextPassword);
        }

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

        public void setUrlIcon(string url)
        {
            //urlIcon = "https://www.google.com/s2/favicons?domain=" + url;
            urlIcon = $"https://t0.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url={url}&size=256";
        }
    }
}
