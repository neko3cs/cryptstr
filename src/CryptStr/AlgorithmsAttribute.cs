using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CryptStr
{
    public class AlgorithmsAttribute : ValidationAttribute
    {
        private static readonly string InvalidMessage =
@"Specified algorithms is not support.
Support algorithms kind is here.
- TripleDES (default)
- DES
- AES256
";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string algorithm && IsSupported(algorithm))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(InvalidMessage);
        }

        internal static bool IsSupported(string value) =>
            Enum.GetNames<SupportAlgorithms>().Contains(value);
    }
}
