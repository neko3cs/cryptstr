using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
            var provider = new TripleDESCryptoServiceProvider();
            var encryptor = provider.CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(iv));

            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();

                Console.WriteLine(Convert.ToBase64String(memoryStream.ToArray()));
            }

            return 0;
        }

        [Command(Description = "Encrypt string of value.")]
        public int Dec(
            [Argument] string value,
            [Option('k')] string key,
            [Option('v')] string iv
        )
        {
            var provider = new TripleDESCryptoServiceProvider();
            var decryptor = provider.CreateDecryptor(Convert.FromBase64String(key), Convert.FromBase64String(iv));

            var bytes = Convert.FromBase64String(value);
            using (var memoryStream = new MemoryStream(bytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                Console.WriteLine(streamReader.ReadLine());
            }

            return 0;
        }

        [Command(Description = "Generate key and IV to file.")]
        public void Gen()
        {
            var provider = new TripleDESCryptoServiceProvider();
            provider.GenerateKey();
            provider.GenerateIV();
            var keyAndIV = new
            {
                Key = Convert.ToBase64String(provider.Key),
                IV = Convert.ToBase64String(provider.IV)
            };
            File.WriteAllText(
                Path.Combine(Directory.GetCurrentDirectory(), "cryptstr.json"),
                JsonConvert.SerializeObject(keyAndIV)
            );
        }
    }
}
