using System.ComponentModel.DataAnnotations;

namespace Flashcards.Core.Services.Helpers
{
	internal static class ValidationHelper
	{
		internal async static Task ValidateObjects(params object?[]? objects)
		{
			if (objects is null)
			{
				throw new ArgumentNullException(nameof(objects));
			}

			foreach (var obj in objects)
			{
				if (obj is null)
				{
					throw new ArgumentNullException(nameof(obj));
				}

				ValidationContext validationContext = new ValidationContext(obj);
				List<ValidationResult> validationResults = new List<ValidationResult>();

				if (!Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true))
				{
					throw new ArgumentException(validationResults.First().ErrorMessage);
				}
			}

			await Task.CompletedTask;
		}
	}
}
