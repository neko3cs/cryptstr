using System.ComponentModel.DataAnnotations;

using Shouldly;

namespace CryptStr.Tests;

public class AlgorithmsAttributeTests
{
    [Theory]
    [InlineData(nameof(SupportAlgorithms.DES))]
    [InlineData(nameof(SupportAlgorithms.TripleDES))]
    public void IsValid_ShouldAcceptSupportedAlgorithms(string value)
    {
        var attribute = new AlgorithmsAttribute();

        var result = attribute.GetValidationResult(value, new ValidationContext(new object()));

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldRejectUnsupportedAlgorithm()
    {
        var attribute = new AlgorithmsAttribute();

        var result = attribute.GetValidationResult("AES", new ValidationContext(new object()));

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldNotBeNull();
        result.ErrorMessage.ShouldContain("Specified algorithms is not support.");
        result.ErrorMessage.ShouldContain("TripleDES");
        result.ErrorMessage.ShouldContain("DES");
    }
}
