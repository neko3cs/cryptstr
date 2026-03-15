using System;
using System.Collections.Generic;

namespace CryptStr
{
    internal static class AlgorithmRegistry
    {
        private sealed record AlgorithmDefinition(
            Func<string, string, ICryptor> CreateCryptor,
            Func<(string Key, string IV)> GenerateKeyAndIV
        );

        private static readonly IReadOnlyDictionary<string, AlgorithmDefinition> Definitions =
            new Dictionary<string, AlgorithmDefinition>(StringComparer.Ordinal)
            {
                [nameof(SupportAlgorithms.TripleDES)] = new(
                    (key, iv) => new TripleDESCryptor(key, iv),
                    TripleDESCryptor.Generate
                ),
                [nameof(SupportAlgorithms.DES)] = new(
                    (key, iv) => new DESCryptor(key, iv),
                    DESCryptor.Generate
                ),
                [nameof(SupportAlgorithms.AES256)] = new(
                    (key, iv) => new AES256Cryptor(key, iv),
                    AES256Cryptor.Generate
                )
            };

        public static IEnumerable<string> SupportedNames => Definitions.Keys;

        public static ICryptor CreateCryptor(string key, string iv, string algorithm) =>
            GetDefinition(algorithm).CreateCryptor(key, iv);

        public static (string Key, string IV) GenerateKeyAndIV(string algorithm) =>
            GetDefinition(algorithm).GenerateKeyAndIV();

        public static bool IsSupported(string algorithm) => Definitions.ContainsKey(algorithm);

        private static AlgorithmDefinition GetDefinition(string algorithm) =>
            Definitions.TryGetValue(algorithm, out var definition)
                ? definition
                : throw new ArgumentException("Unsupported algorithms.", nameof(algorithm));
    }
}
