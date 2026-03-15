using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CryptStr
{
    public class AlgorithmsAttribute : ValidationAttribute
    {
        private static readonly string InvalidMessage = CreateInvalidMessage();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string algorithm && IsSupported(algorithm))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(InvalidMessage);
        }

        private static string CreateInvalidMessage() =>
$@"Specified algorithms is not support.
Support algorithms kind is here.
{string.Join(Environment.NewLine, AlgorithmRegistry.SupportedNames.Select(static algorithm => $"- {algorithm}"))}
";

        internal static bool IsSupported(string value) => AlgorithmRegistry.IsSupported(value);
    }
}
