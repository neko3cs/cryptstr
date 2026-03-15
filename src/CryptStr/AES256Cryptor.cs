using System.Security.Cryptography;

namespace CryptStr
{
    public class AES256Cryptor(string key, string iv) : SymmetricCryptorBase(key, iv)
    {
        protected override SymmetricAlgorithm CreateAlgorithm() => Aes.Create();

        public static (string Key, string IV) Generate() => GenerateKeyAndIV(Aes.Create);
    }
}
