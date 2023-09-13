using Konscious.Security.Cryptography;
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
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }

        public List<Credentials> UserCredentials { get; set; }

        public string Salt { get; set; }

        private Argon2Hasher _argon2Hasher = new Argon2Hasher();

        

        public User()
        {
            CreatedDate = DateTime.UtcNow;
            UserCredentials = new List<Credentials>();
            IsLocked = false;
            FailedLoginAttempts = 0;
        }

        public void SetPassword(string plainTextPassword)
        {
            byte[] salt;
            EncryptedPassword = _argon2Hasher.HashPassword(plainTextPassword, out salt);
            Salt = Convert.ToBase64String(salt);  // Conversion en string
        }

        public bool Authenticate(string plainTextPassword)
        {
            byte[] saltBytes = Convert.FromBase64String(Salt);  // Conversion en byte[]
            return _argon2Hasher.VerifyPassword(EncryptedPassword, plainTextPassword, saltBytes);
        }
    }
}
