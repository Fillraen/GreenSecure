using Konscious.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;

public class Argon2Hasher
{
    public string HashPassword(string password, out byte[] salt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Générer un sel aléatoire
        salt = new byte[8];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        var argon2 = new Argon2id(passwordBytes)
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            MemorySize = 8192,
            Iterations = 4
        };

        byte[] hashBytes = argon2.GetBytes(16);
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string hashedPassword, string plainTextPassword, byte[] salt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(plainTextPassword);
        var argon2 = new Argon2id(passwordBytes)
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            MemorySize = 8192,
            Iterations = 4
        };

        byte[] hashBytes = argon2.GetBytes(16);
        string newHashedPassword = Convert.ToBase64String(hashBytes);

        return hashedPassword == newHashedPassword;
    }
}