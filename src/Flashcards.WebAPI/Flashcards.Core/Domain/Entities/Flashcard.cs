using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Core.Domain.Entities
{
	public class Flashcard
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId _id { get; set; }

		public Guid CardId { get; set; }

		public Guid UserId { get; set; }

		public float EFactor { get; set; } = 2.5f;

		public int RepetitionCount { get; set; } = 0;

		public DateOnly NextRepeatDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

		[StringLength(100)]
		public string MainSide { get; set; }

		[StringLength(500)]
		public string OppositeSide { get; set; }
	}
}
