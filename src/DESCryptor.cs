using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptStr
{
    public class DESCryptor : ICryptor
    {
        private readonly string Key;
        private readonly string IV;

        public DESCryptor(string key, string iv)
        {
            Key = key;
            IV = iv;
        }

        public string Encrypt(string value)
        {
            var provider = new DESCryptoServiceProvider();
            var encryptor = provider.CreateEncryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV));

            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public string Decrypt(string value)
        {
            var provider = new DESCryptoServiceProvider();
            var decryptor = provider.CreateDecryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV));

            var bytes = Convert.FromBase64String(value);
            using (var memoryStream = new MemoryStream(bytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadLine();
            }
        }

        public static (string Key, string IV) Generate()
        {
            var provider = new DESCryptoServiceProvider();
            provider.GenerateKey();
            provider.GenerateIV();

            return (
                Key: Convert.ToBase64String(provider.Key),
                IV: Convert.ToBase64String(provider.IV)
            );
        }
    }
}
