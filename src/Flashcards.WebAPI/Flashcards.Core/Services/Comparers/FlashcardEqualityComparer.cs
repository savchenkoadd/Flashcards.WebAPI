using Flashcards.Core.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Flashcards.Core.Services.Comparers
{
	public class FlashcardEqualityComparer : IEqualityComparer<Flashcard>
	{
		public bool Equals(Flashcard? x, Flashcard? y)
		{
			return x.CardId.Equals(y.CardId);
		}

		public int GetHashCode([DisallowNull] Flashcard obj)
		{
			return obj.CardId.GetHashCode();
		}
	}
}
