using System;

using Shouldly;

namespace CryptStr.Tests;

public class AlgorithmRegistryTests
{
    [Theory]
    [InlineData(nameof(SupportAlgorithms.DES), typeof(DESCryptor))]
    [InlineData(nameof(SupportAlgorithms.TripleDES), typeof(TripleDESCryptor))]
    [InlineData(nameof(SupportAlgorithms.AES256), typeof(AES256Cryptor))]
    public void CreateCryptor_ShouldReturnExpectedCryptorType(string algorithm, Type expectedType)
    {
        var (key, iv) = AlgorithmRegistry.GenerateKeyAndIV(algorithm);

        var cryptor = AlgorithmRegistry.CreateCryptor(key, iv, algorithm);

        cryptor.ShouldBeOfType(expectedType);
    }

    [Theory]
    [InlineData("Unsupported")]
    [InlineData("")]
    public void GenerateKeyAndIV_ShouldThrowForUnsupportedAlgorithm(string algorithm)
    {
        var exception = Should.Throw<ArgumentException>(() => AlgorithmRegistry.GenerateKeyAndIV(algorithm));

        exception.ParamName.ShouldBe("algorithm");
        exception.Message.ShouldContain("Unsupported algorithms.");
    }
}
