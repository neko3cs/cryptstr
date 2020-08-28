using System;
using System.IO;
using Cocona;
using Newtonsoft.Json;

namespace CryptStr
{
    class Program
    {
        static void Main(string[] args)
        {
            CoconaApp.Run<Program>(args);
        }

        [Command(Description = "Encrypt string of value.")]
        public int Enc(
            [Argument] string value,
            [Option('k')] string key,
            [Option('v')] string iv
        )
        {
            var cryptor = new TripleDESCryptor(key, iv);
            Console.WriteLine(cryptor.Encrypt(value));

            return 0;
        }

        [Command(Description = "Encrypt string of value.")]
        public int Dec(
            [Argument] string value,
            [Option('k')] string key,
            [Option('v')] string iv
        )
        {
            var cryptor = new TripleDESCryptor(key, iv);
            Console.WriteLine(cryptor.Decrypt(value));

            return 0;
        }

        [Command(Description = "Generate key and IV to file.")]
        public void Gen()
        {
            var keyAndIV = TripleDESCryptor.Generate();
            File.WriteAllText(
                Path.Combine(Directory.GetCurrentDirectory(), "cryptstr.json"),
                JsonConvert.SerializeObject(new
                {
                    keyAndIV.Key,
                    keyAndIV.IV
                })
            );
        }
    }
}
