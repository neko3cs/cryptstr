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
";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Enum.GetNames(typeof(SupportAlgorithms)).Contains((string)value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(InvalidMessage);
        }
    }
}
