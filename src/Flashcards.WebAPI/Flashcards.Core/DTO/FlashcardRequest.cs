using MongoDB.Bson;
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

		[Required]
		[Range(0, float.MaxValue)]
		public float EFactor { get; set; } 

		[Required]
		[Range(0, int.MaxValue)]
		public int RepetitionCount { get; set; }

		[Required]
		[DataType(DataType.Date, ErrorMessage = "DateTime is not valid.")]
		public DateOnly NextRepeatDate { get; set; }

		public bool WhetherToDelete { get; set; } = false;	
	}
}
