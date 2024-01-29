using System.ComponentModel.DataAnnotations;

namespace Flashcards.Core.DTO
{
	public class FlashcardRequest
	{
		[Required]
		public Guid CardId { get; set; }

		[Required]
		[StringLength(100)]
		public string? MainSide { get; set; }

		[Required]
		[StringLength(500)]
		public string? OppositeSide { get; set; }

		[Range(0, float.MaxValue)]
		public float EFactor { get; set; } = 2.5f;

		[Range(0, int.MaxValue)]
		public int RepetitionCount { get; set; } = 0;

		public DateOnly NextRepeatDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
	}
}
