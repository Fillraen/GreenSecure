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
       
        public string CreatedDate { get; set; }
        public string LastLoginDate { get; set; }
        public int IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }

        public List<Credentials> UserCredentials { get; set; }

        public string EncryptionKey { get; set; }
        public string EncryptionIV { get; set; }

        public User()
        {
            CreatedDate = DateTime.UtcNow.ToString();
            UserCredentials = new List<Credentials>();
            IsLocked = 0;
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
                    EncryptionIV = Convert.ToBase64String(aes.IV);
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
}
