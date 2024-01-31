using Flashcards.Core.Domain.Entities;
using Flashcards.Core.Domain.RepositoryContracts;
using Flashcards.Core.DTO;

namespace Flashcards.Core.Services.Helpers
{
	internal class SyncHelper
	{
		private readonly Guid _userId;
		private readonly IRepository<Flashcard> _repository;
		private readonly IEqualityComparer<Flashcard> _equalityComparer;

		public SyncHelper(
				Guid userId,
				IRepository<Flashcard> repository,
				IEqualityComparer<Flashcard> equalityComparer
			)
		{
			_repository = repository;
			_userId = userId;
			_equalityComparer = equalityComparer;
		}

		internal async Task<IEnumerable<Flashcard>> ConvertRequests(IEnumerable<FlashcardRequest> userCards)
		{
			return await Task.FromResult(userCards.Select(temp => new Flashcard()
			{
				CardId = temp.CardId,
				EFactor = temp.EFactor,
				NextRepeatDate = temp.NextRepeatDate,
				MainSide = temp.MainSide,
				OppositeSide = temp.OppositeSide,
				RepetitionCount = temp.RepetitionCount,
				UserId = _userId
			}));
		}

		internal async Task<IEnumerable<Guid>> RetrieveCardsIdsToBeDeleted(IEnumerable<FlashcardRequest> userFlashcards)
		{
			return await Task.FromResult(userFlashcards!.Where(temp => temp.WhetherToDelete == true).Select(temp => temp.CardId));
		}

		/// <summary>
		/// </summary>
		/// <param name="localCards"></param>
		/// <param name="userCards"></param>
		/// <returns>Bool, indicating if any card has been updated</returns>
		internal async Task<bool> UpdateCards(IEnumerable<Flashcard> localCards, IEnumerable<Flashcard> userCards)
		{
			var updatedAnyCard = false;

			foreach (var flashcard in userCards)
			{
				if (localCards.Contains(flashcard, _equalityComparer))
				{
					//TO DO: implement more reasonable updating
					await _repository.UpdateAsync(temp => temp.CardId == flashcard.CardId, flashcard);
					updatedAnyCard = true;
				}
			}

			return updatedAnyCard;
		}

		internal async Task<HashSet<Flashcard>> RetrieveCardsFromStorage()
		{
			return (await _repository.GetAllAsync(temp => temp.UserId == _userId)).ToHashSet();
		}

		internal async Task<HashSet<Flashcard>> UnionLocalCardsWithUserCards(IEnumerable<Flashcard> localCards, IEnumerable<Flashcard> userCards)
		{
			return await Task.FromResult(localCards.Union(userCards, _equalityComparer).ToHashSet());
		}

		internal async Task<IEnumerable<Flashcard>> GetCardsToCreate(IEnumerable<Flashcard> unionResult, IEnumerable<Flashcard> localCards)
		{
			return await Task.FromResult(unionResult.Except(localCards, _equalityComparer));
		}
	}
}
