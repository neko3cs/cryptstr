using System;
using System.IO;
using System.Text.Json;

using Shouldly;

namespace CryptStr.Tests;

public class ProgramTests
{
    [Fact]
    public void Main_ShouldCreateCryptStrJsonForDefaultAlgorithm()
    {
        var originalCurrentDirectory = Directory.GetCurrentDirectory();
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryDirectory);

        try
        {
            Directory.SetCurrentDirectory(temporaryDirectory);

            Program.Main(["gen"]);

            var filePath = Path.Combine(temporaryDirectory, "cryptstr.json");
            File.Exists(filePath).ShouldBeTrue();

            using var document = JsonDocument.Parse(File.ReadAllText(filePath));
            Convert.FromBase64String(document.RootElement.GetProperty("Key").GetString()!).Length.ShouldBeGreaterThan(0);
            Convert.FromBase64String(document.RootElement.GetProperty("IV").GetString()!).Length.ShouldBeGreaterThan(0);
        }
        finally
        {
            Directory.SetCurrentDirectory(originalCurrentDirectory);
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Fact]
    public void Encrypt_ShouldWriteEncryptedValueToConsole()
    {
        var (key, iv) = TripleDESCryptor.Generate();

        var (result, output) = CaptureConsoleOutput(() =>
            Program.Encrypt("hello world", key, iv, nameof(SupportAlgorithms.TripleDES))
        );

        result.ShouldBe(0);
        output.Trim().ShouldNotBeNullOrEmpty();
        output.Trim().ShouldNotBe("hello world");
    }

    [Fact]
    public void Decrypt_ShouldWritePlainTextToConsole()
    {
        var plainText = "hello world";
        var (key, iv) = TripleDESCryptor.Generate();
        var cryptor = new TripleDESCryptor(key, iv);
        var encrypted = cryptor.Encrypt(plainText);

        var (result, output) = CaptureConsoleOutput(() =>
            Program.Decrypt(encrypted, key, iv, nameof(SupportAlgorithms.TripleDES))
        );

        result.ShouldBe(0);
        output.Trim().ShouldBe(plainText);
    }

    [Fact]
    public void Encrypt_ShouldThrowForUnsupportedAlgorithm()
    {
        var exception = Should.Throw<ArgumentException>(() =>
            Program.Encrypt("hello world", "unused", "unused", "Unsupported")
        );

        exception.ParamName.ShouldBe("algorithm");
        exception.Message.ShouldContain("Unsupported algorithms.");
    }

    private static (int Result, string Output) CaptureConsoleOutput(Func<int> action)
    {
        var originalOutput = Console.Out;
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        try
        {
            return (action(), stringWriter.ToString());
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }
}
