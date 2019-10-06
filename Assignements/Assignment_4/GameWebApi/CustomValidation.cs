using System;
using System.ComponentModel.DataAnnotations;

public class CustomValidation
{
    public sealed class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentDateTime = new DateTime();

            if ((DateTime)value > currentDateTime)
            {
                return new ValidationResult($"Date cannot be newer than the current date {value}.");
            }

            return ValidationResult.Success;
        }
    }
}