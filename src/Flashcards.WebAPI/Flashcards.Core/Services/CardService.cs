using Flashcards.Core.Domain.Entities;
using Flashcards.Core.Domain.RepositoryContracts;
using Flashcards.Core.DTO;
using Flashcards.Core.ServiceContracts;
using Flashcards.Core.Services.Comparers;
using Flashcards.Core.Services.Helpers;

namespace Flashcards.Core.Services
{
	public class CardService : ICardService
	{
		private readonly IRepository<Flashcard> _repository;

		public CardService(IRepository<Flashcard> repository)
		{
			_repository = repository;
		}

		public async Task<List<FlashcardResponse>> GetAllAsync(Guid? userId)
		{
			await ValidationHelper.ValidateObjects(userId);

			return (await _repository.GetAllAsync(temp => temp.UserId == userId)).Select(temp => new FlashcardResponse()
			{
				CardId = temp.CardId,
				EFactor = temp.EFactor,
				MainSide = temp.MainSide,
				OppositeSide = temp.OppositeSide,
				NextRepeatDate = temp.NextRepeatDate,
				RepetitionCount = temp.RepetitionCount
			}).ToList();
		}

		public async Task<AffectedResponse> SyncCards(Guid? userId, IEnumerable<FlashcardRequest>? flashcards)
		{
			// Validate input parameters
			await ValidationHelper.ValidateObjects(flashcards, userId);

			var totallyAffected = 0;
			var cards = flashcards.Select(item => new Flashcard
			{
				CardId = item.CardId,
				EFactor = item.EFactor,
				MainSide = item.MainSide,
				OppositeSide = item.OppositeSide,
				NextRepeatDate = item.NextRepeatDate,
				RepetitionCount = item.RepetitionCount,
				UserId = userId.Value
			}).ToList();

			// Retrieve all cards for the user
			var allCards = await _repository.GetAllAsync(temp => temp.UserId == userId);

			foreach (var item in cards)
			{
				// Check if the card already exists
				var existingCard = allCards.FirstOrDefault(temp => temp.CardId == item.CardId);

				if (existingCard == null)
				{
					// Create a new card
					await _repository.CreateAsync(item);
					totallyAffected++;
				}
				else
				{
					// Update existing card
					totallyAffected += await _repository.UpdateAsync(temp => temp.CardId == item.CardId, item);
				}
			}

			// Identify cards to delete
			var cardsToDelete = allCards.Except(cards, new FlashcardEqualityComparer());

			// Delete cards
			foreach (var card in cardsToDelete)
			{
				totallyAffected += await _repository.DeleteAsync(temp => temp.CardId == card.CardId);
			}

			return new AffectedResponse { Affected = totallyAffected };
		}
	}
}
