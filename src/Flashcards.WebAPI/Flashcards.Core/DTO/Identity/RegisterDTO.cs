using System.ComponentModel.DataAnnotations;

namespace Flashcards.Core.DTO.Identity
{
	public class RegisterDTO
	{
		[Required]
		public string? PersonName { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		public string? Email { get; set; } = string.Empty;

		[Required]
		[Phone]
		public string? PhoneNumber { get; set; } = string.Empty;

		[Required]
		public string? Password {  get; set; } = string.Empty;

		[Required]
		[Compare(nameof(Password))]
		public string? ConfirmPassword { get; set; } = string.Empty;
	}
}
