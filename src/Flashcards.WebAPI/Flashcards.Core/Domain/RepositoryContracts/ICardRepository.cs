using Flashcards.Core.Domain.Entities;

namespace Flashcards.Core.Domain.RepositoryContracts
{
	public interface ICardRepository
	{
		Task<int> SyncCards(List<Flashcard> flashcards);
	}
}
