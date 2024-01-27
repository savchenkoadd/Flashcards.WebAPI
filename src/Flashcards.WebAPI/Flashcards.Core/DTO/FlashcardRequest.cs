namespace Flashcards.Core.DTO
{
	public class FlashcardRequest
	{
		public Guid CardId { get; set; }

		public string MainSide { get; set; }

		public string OppositeSide { get; set; }

		public float EFactor { get; set; } = 2.5f;

		public int RepetitionCount { get; set; } = 0;

		public int RepeatInDays { get; set; } = 0;
	}
}
