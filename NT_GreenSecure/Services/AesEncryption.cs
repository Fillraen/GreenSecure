// Classe AesEncryption - Gère le chiffrement et le déchiffrement des mots de passe
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
            // Constructeur par défaut de la classe
        }

        // Méthode pour chiffrer un mot de passe
        protected string EncryptPassword(string plainText, string keyBase64, string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                // Configuration des paramètres de chiffrement
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);

                // Création de l'objet encryptor
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

                byte[] encryptedData;

                // Le chiffrement est effectué dans un MemoryStream via un objet CryptoStream
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

                // Retourne le texte chiffré en base64
                return Convert.ToBase64String(encryptedData);
            }
        }

        // Méthode pour déchiffrer un mot de passe
        protected string DecryptPassword(string cipherText, string keyBase64, string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                // Configuration des paramètres de chiffrement
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);

                // Création de l'objet decryptor
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] decryptedData;

                // Le déchiffrement est effectué dans un MemoryStream ( tampon de mémoire ) via un objet CryptoStream
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

                // Retourne le texte déchiffré en UTF-8
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        // Méthode pour vérifier si un mot de passe chiffré correspond à un mot de passe en clair
        protected bool VerifyPassword(string encryptPassword, string plainTextPassword, string keyBase64, string vectorBase64)
        {
            string newEncryptedPassword = EncryptPassword(plainTextPassword, keyBase64, vectorBase64);
            bool result = encryptPassword == newEncryptedPassword;
            return result;
        }
    }
}
