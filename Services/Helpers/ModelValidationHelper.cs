using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public static class ModelValidationHelper
    {
        public static void ModelValidation(object model)
        {
            ValidationContext validationContext = new ValidationContext(model);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, validationContext,
                validationResults, validateAllProperties: true);

            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
