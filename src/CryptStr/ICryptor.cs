namespace CryptStr
{
    public interface ICryptor
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
