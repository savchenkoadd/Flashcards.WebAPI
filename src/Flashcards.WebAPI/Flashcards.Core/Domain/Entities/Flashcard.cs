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

		public float EFactor { get; set; }

		public int RepetitionCount { get; set; }

		public DateOnly NextRepeatDate { get; set; }

		[StringLength(100)]
		public string MainSide { get; set; }

		[StringLength(500)]
		public string OppositeSide { get; set; }
	}
}
