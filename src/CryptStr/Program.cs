using System;
using System.IO;
using System.Text.Json;
using ConsoleAppFramework;

namespace CryptStr
{
    internal static class Program
    {
        private const string DefaultAlgorithms = nameof(SupportAlgorithms.TripleDES);

        public static void Main(string[] args)
        {
            var app = ConsoleApp.Create();
            app.Add("enc", Encrypt);
            app.Add("dec", Decrypt);
            app.Add("gen", Generate);
            app.Run(args);
        }

        /// <summary>
        /// Encrypt string of value.
        /// </summary>
        /// <param name="value">Plain string value.</param>
        /// <param name="key">-k, Encryption key.</param>
        /// <param name="iv">-v, Initialization vector.</param>
        /// <param name="algorithms">-a, Encryption algorithm.</param>
        public static int Encrypt(
            [Argument] string value,
            string key,
            string iv,
            [Algorithms] string algorithms = DefaultAlgorithms
        )
        {
            var cryptor = CreateCryptor(key, iv, algorithms);
            Console.WriteLine(cryptor.Encrypt(value));
            return 0;
        }

        /// <summary>
        /// Decrypt string of value.
        /// </summary>
        /// <param name="value">Encrypted string value.</param>
        /// <param name="key">-k, Encryption key.</param>
        /// <param name="iv">-v, Initialization vector.</param>
        /// <param name="algorithms">-a, Encryption algorithm.</param>
        public static int Decrypt(
            [Argument] string value,
            string key,
            string iv,
            [Algorithms] string algorithms = DefaultAlgorithms
        )
        {
            var cryptor = CreateCryptor(key, iv, algorithms);
            Console.WriteLine(cryptor.Decrypt(value));
            return 0;
        }

        /// <summary>
        /// Generate key and IV to file.
        /// </summary>
        /// <param name="algorithms">-a, Encryption algorithm.</param>
        public static void Generate([Algorithms] string algorithms = DefaultAlgorithms)
        {
            var (key, iv) = GenerateKeyAndIV(algorithms);
            File.WriteAllText(
                Path.Combine(Directory.GetCurrentDirectory(), "cryptstr.json"),
                JsonSerializer.Serialize(new
                {
                    Key = key,
                    IV = iv
                })
            );
        }

        private static ICryptor CreateCryptor(string key, string iv, string algorithms) => algorithms switch
        {
            nameof(SupportAlgorithms.TripleDES) => new TripleDESCryptor(key, iv),
            nameof(SupportAlgorithms.DES) => new DESCryptor(key, iv),
            _ => throw new ArgumentException("Unsupported algorithms.")
        };

        private static (string Key, string IV) GenerateKeyAndIV(string algorithms) => algorithms switch
        {
            nameof(SupportAlgorithms.TripleDES) => TripleDESCryptor.Generate(),
            nameof(SupportAlgorithms.DES) => DESCryptor.Generate(),
            _ => throw new ArgumentException("Unsupported algorithms.")
        };
    }
}
