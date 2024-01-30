using AutoMapper;
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
		private readonly IMapper _mapper;

		public CardService(
				IRepository<Flashcard> repository,
				IMapper mapper
			)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<FlashcardResponse>> GetAllAsync(Guid? userId)
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
			});
		}

		public async Task<IEnumerable<FlashcardResponse>> SyncAndGetCards(Guid? userId, IEnumerable<FlashcardRequest>? flashcards)
		{
			await ValidationHelper.ValidateObjects(userId, flashcards);

			var localCards = (await _repository.GetAllAsync(temp => true)).ToHashSet();

			if (localCards is null)
			{
				throw new NullReferenceException("Unable to retrieve local cards.");
			}

		}

		public async Task<AffectedResponse> SyncCards(Guid? userId, IEnumerable<FlashcardRequest>? flashcards)
		{
			// Validate input parameters
			await ValidationHelper.ValidateObjects(flashcards, userId);

			var totallyAffected = 0;

			var cards = _mapper.Map<List<Flashcard>>(flashcards);

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
