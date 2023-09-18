using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace NT_GreenSecure.Services
{
    public class AesEncryption
    {
        public AesEncryption()
        {

        }

        public string EncryptPassword(string plainText, string keyBase64, string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {

                //set the parameters with out keyword
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);

                // Create encryptor object
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

                byte[] encryptedData;

                //Encryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryptedData = ms.ToArray();
                    }
                }

                return Convert.ToBase64String(encryptedData);
            }
        }

        public string DecryptPassword(string cipherText, string keyBase64, string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                //set the parameters with out keyword
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);

                // Create decryptor object
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] decryptedData;

                //Decryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decryptedData = Encoding.UTF8.GetBytes(sr.ReadToEnd());
                        }
                    }
                }

                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public bool VerifyPassword(string encryptPassword, string plainTextPassword, string keyBase64, string vectorBase64)
        {
            string decryptedPassword = DecryptPassword(encryptPassword, keyBase64, vectorBase64);
            return decryptedPassword == plainTextPassword;
        }

    }
}
