﻿using Flashcards.Core.DTO;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Flashcards.Core.ServiceContracts
{
	public interface ICardService
	{
		Task<AffectedResponse> SyncCards(Guid? userId, IEnumerable<FlashcardRequest>? flashcards);

		Task<IEnumerable<FlashcardResponse>> GetAllAsync(Guid? userId);

		Task<IEnumerable<FlashcardResponse>> SyncAndGetCards(Guid? userId, IEnumerable<FlashcardRequest>? flashcards);

		Task<AffectedResponse> DeleteCards(Guid[]? cardIds);
	}
}
