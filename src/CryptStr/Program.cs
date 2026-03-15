using System;
using System.IO;
using Cocona;
using Newtonsoft.Json;

namespace CryptStr
{
    class Program
    {
        private const string DefaultAlgorithms = "TripleDES";

        static void Main(string[] args) => CoconaApp.Run<Program>(args);

        [Command(Description = "Encrypt string of value.")]
        public static int Enc(
            [Argument] string value,
            [Option('k')] string key,
            [Option('v')] string iv,
            [Option('a')][Algorithms] string algorithms = DefaultAlgorithms
        )
        {
            ICryptor cryptor = algorithms switch
            {
                nameof(SupportAlgorithms.TripleDES) => new TripleDESCryptor(key, iv),
                nameof(SupportAlgorithms.DES) => new DESCryptor(key, iv),
                _ => throw new ArgumentException("Unsupported algorithms.")
            };
            Console.WriteLine(cryptor.Encrypt(value));

            return 0;
        }

        [Command(Description = "Decrypt string of value.")]
        public static int Dec(
            [Argument] string value,
            [Option('k')] string key,
            [Option('v')] string iv,
            [Option('a')][Algorithms] string algorithms = DefaultAlgorithms
        )
        {
            ICryptor cryptor = algorithms switch
            {
                nameof(SupportAlgorithms.TripleDES) => new TripleDESCryptor(key, iv),
                nameof(SupportAlgorithms.DES) => new DESCryptor(key, iv),
                _ => throw new ArgumentException("Unsupported algorithms.")
            };
            Console.WriteLine(cryptor.Decrypt(value));

            return 0;
        }

        [Command(Description = "Generate key and IV to file.")]
        public static void Gen(
            [Option('a')][Algorithms] string algorithms = DefaultAlgorithms
        )
        {

            var (Key, IV) = algorithms switch
            {
                nameof(SupportAlgorithms.TripleDES) => TripleDESCryptor.Generate(),
                nameof(SupportAlgorithms.DES) => DESCryptor.Generate(),
                _ => throw new ArgumentException("Unsupported algorithms.")
            };
            File.WriteAllText(
                Path.Combine(Directory.GetCurrentDirectory(), "cryptstr.json"),
                JsonConvert.SerializeObject(new
                {
                    Key,
                    IV
                })
            );
        }
    }
}
