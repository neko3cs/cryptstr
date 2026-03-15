using System.Security.Cryptography;

namespace CryptStr
{
    public class TripleDESCryptor(string key, string iv) : SymmetricCryptorBase(key, iv)
    {
        protected override SymmetricAlgorithm CreateAlgorithm() => TripleDES.Create();

        public static (string Key, string IV) Generate() => GenerateKeyAndIV(TripleDES.Create);
    }
}
