using System;
using System.Collections.Generic;

using Shouldly;

namespace CryptStr.Tests;

public class CryptorTests
{
    public static TheoryData<string> PlainTextValues => new()
    {
        string.Empty,
        "hello world",
        "1234567890",
        "こんにちは、CryptStr!",
        "line1\nline2\nline3",
        "symbols !@#$%^&*()_+-=[]{};':\",.<>/?\tend"
    };

    [Theory]
    [MemberData(nameof(PlainTextValues))]
    public void DESCryptor_ShouldRoundTripPlainText(string value)
    {
        var (key, iv) = DESCryptor.Generate();
        var cryptor = new DESCryptor(key, iv);

        var encrypted = cryptor.Encrypt(value);
        var decrypted = cryptor.Decrypt(encrypted);

        encrypted.ShouldNotBeNullOrEmpty();
        decrypted.ShouldBe(value);
    }

    [Theory]
    [MemberData(nameof(PlainTextValues))]
    public void TripleDESCryptor_ShouldRoundTripPlainText(string value)
    {
        var (key, iv) = TripleDESCryptor.Generate();
        var cryptor = new TripleDESCryptor(key, iv);

        var encrypted = cryptor.Encrypt(value);
        var decrypted = cryptor.Decrypt(encrypted);

        encrypted.ShouldNotBeNullOrEmpty();
        decrypted.ShouldBe(value);
    }

    [Fact]
    public void DESGenerate_ShouldReturnBase64EncodedKeyAndIVWithExpectedLengths()
    {
        var (key, iv) = DESCryptor.Generate();

        Convert.FromBase64String(key).Length.ShouldBe(8);
        Convert.FromBase64String(iv).Length.ShouldBe(8);
    }

    [Fact]
    public void TripleDESGenerate_ShouldReturnBase64EncodedKeyAndIVWithExpectedLengths()
    {
        var (key, iv) = TripleDESCryptor.Generate();

        var keyBytes = Convert.FromBase64String(key);
        var ivBytes = Convert.FromBase64String(iv);

        ivBytes.Length.ShouldBe(8);
        new[] { 16, 24 }.ShouldContain(keyBytes.Length);
    }
}
