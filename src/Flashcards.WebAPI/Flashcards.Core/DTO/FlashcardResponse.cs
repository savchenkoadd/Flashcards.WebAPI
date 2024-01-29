using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Core.DTO
{
	public class FlashcardResponse
	{
		public Guid CardId { get; set; }

		public string? MainSide { get; set; }

		public string? OppositeSide { get; set; }

		public float EFactor { get; set; } = 2.5f;

		public int RepetitionCount { get; set; } = 0;

		public DateOnly NextRepeatDate { get; set; }
	}
}
