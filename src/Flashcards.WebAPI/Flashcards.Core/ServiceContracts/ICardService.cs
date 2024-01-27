using Flashcards.Core.DTO;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Flashcards.Core.ServiceContracts
{
	public interface ICardService
	{
		Task<int> SyncCards(IReadOnlyCollection<FlashcardRequest>? flashcards);

		Task<IReadOnlyCollection<FlashcardResponse>> GetAllAsync(Guid? userId);

		Task<IReadOnlyCollection<FlashcardResponse>> DeleteAllAsync(List<Guid>? cardsIds);
	}
}
