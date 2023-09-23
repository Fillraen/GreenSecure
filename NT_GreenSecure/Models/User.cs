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
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string EncryptedPassword { get; set; }
       
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }

        public List<Credentials> UserCredentials { get; set; }

        protected string EncryptionKey;
        protected string EncryptionIV;


        public User()
        {
            CreatedDate = DateTime.UtcNow;
            UserCredentials = new List<Credentials>();
            IsLocked = false;
            FailedLoginAttempts = 0;

            
        }
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

        public void SetEncryptionIV(string iv = null)
        {

            if (iv == null)
            {
                using (Aes aes = Aes.Create())
                {
                    EncryptionIV = Convert.ToBase64String(aes.Key);
                }
            }
            else
            {
                EncryptionIV = iv;
            }
        }

        public string GetEncryptionKey()
        {
            return EncryptionKey;
        }

        public string GetEncryptionIV()
        {
            return EncryptionIV;
        }

        public void SetEncryptedPassword(string plainTextPassword)
        {
            EncryptedPassword = EncryptPassword(plainTextPassword, EncryptionKey, EncryptionIV);
        }

        public string GetPlainTextPassword()
        {
            return DecryptPassword(EncryptedPassword, EncryptionKey, EncryptionIV);
        }
    }

    public class UserJSON
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

        public string EncryptionKey { get; set; }
        public string EncryptionIV { get; set; }
    }
}
