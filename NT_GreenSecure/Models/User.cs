using Konscious.Security.Cryptography;
using NT_GreenSecure.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NT_GreenSecure.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string EncryptedPassword { get; set; }
        public string EncryptionKey { get; set; }
        public string EncryptionIV { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }

        public List<Credentials> UserCredentials { get; set; }
        

        public User()
        {
            CreatedDate = DateTime.UtcNow;
            UserCredentials = new List<Credentials>();
            IsLocked = false;
            FailedLoginAttempts = 0;

            using (Aes aes = Aes.Create())
            {
                EncryptionKey = Convert.ToBase64String(aes.Key);
                EncryptionIV = Convert.ToBase64String(aes.IV);
            }
        }
    }
}
