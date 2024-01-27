using Flashcards.Core.Domain.Entities;
using Flashcards.Core.Domain.RepositoryContracts;
using Flashcards.Core.DTO;
using Flashcards.Core.ServiceContracts;
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
				RepeatInDays = temp.RepeatInDays,
				RepetitionCount = temp.RepetitionCount
			}).ToList();
		}

		public async Task<AffectedResponse> SyncCards(Guid? userId, IEnumerable<FlashcardRequest>? flashcards)
		{
			await ValidationHelper.ValidateObjects(flashcards, userId);

			var totallyAffected = 0;
			var cards = new List<Flashcard>();

			foreach (var item in flashcards)
			{
				cards.Add(new Flashcard()
				{
					CardId = item.CardId,
					EFactor = item.EFactor,
					MainSide = item.MainSide,
					OppositeSide = item.OppositeSide,
					RepeatInDays = item.RepeatInDays,
					RepetitionCount = item.RepetitionCount,
					UserId = userId.Value
				});
			}

			var allCards = await _repository.GetAllAsync(temp => temp.UserId == userId);

			foreach (var item in cards)
			{
				if (!allCards.Contains(item))
				{
					await _repository.CreateAsync(item);
					totallyAffected++;
				}
				else
				{
					totallyAffected += await _repository.UpdateAsync(temp => temp.CardId == item.CardId, item);
				}
			}

			var cardsToDelete = allCards.Where(temp => !cards.Contains(temp));

			foreach (var card in cardsToDelete)
			{
				totallyAffected += await _repository.DeleteAsync(temp => temp.CardId == card.CardId);
			}

			return new AffectedResponse() { Affected = totallyAffected };
		}
	}
}
