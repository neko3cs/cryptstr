using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptStr
{
    public abstract class SymmetricCryptorBase(string key, string iv) : ICryptor
    {
        private readonly byte[] KeyBytes = Convert.FromBase64String(key);
        private readonly byte[] IVBytes = Convert.FromBase64String(iv);

        public string Encrypt(string value)
        {
            using var provider = CreateAlgorithm();
            using var encryptor = provider.CreateEncryptor(KeyBytes, IVBytes);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            var bytes = Encoding.UTF8.GetBytes(value);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public string Decrypt(string value)
        {
            using var provider = CreateAlgorithm();
            using var decryptor = provider.CreateDecryptor(KeyBytes, IVBytes);

            var bytes = Convert.FromBase64String(value);
            using var memoryStream = new MemoryStream(bytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }

        protected abstract SymmetricAlgorithm CreateAlgorithm();

        protected static (string Key, string IV) GenerateKeyAndIV(
            Func<SymmetricAlgorithm> algorithmFactory
        )
        {
            using var provider = algorithmFactory();

            return (
                Key: Convert.ToBase64String(provider.Key),
                IV: Convert.ToBase64String(provider.IV)
            );
        }
    }
}
