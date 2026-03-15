using System.Security.Cryptography;

namespace CryptStr
{
    public class DESCryptor(string key, string iv) : SymmetricCryptorBase(key, iv)
    {
        protected override SymmetricAlgorithm CreateAlgorithm() => DES.Create();

        public static (string Key, string IV) Generate() => GenerateKeyAndIV(DES.Create);
    }
}
