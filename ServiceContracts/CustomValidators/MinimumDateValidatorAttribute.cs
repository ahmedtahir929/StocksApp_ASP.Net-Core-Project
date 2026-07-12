using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ServiceContracts.CustomValidators
{
    public class MinimumDateValidatorAttribute : ValidationAttribute
    {
        private readonly DateTime? _minimumDate;

        public MinimumDateValidatorAttribute(string date)
        {
            _minimumDate = DateTime.ParseExact(date, "MMM dd, yyyy", CultureInfo.InvariantCulture);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is DateTime dateTime && dateTime < _minimumDate)
            {
                return new ValidationResult(ErrorMessage
                    ?? $"Date cannot be older than {_minimumDate.Value.ToShortDateString()}");
            }
            return ValidationResult.Success;
        }
    }
}
