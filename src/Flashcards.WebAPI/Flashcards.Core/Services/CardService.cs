using AutoMapper;
using Flashcards.Core.Domain.Entities;
using Flashcards.Core.Domain.RepositoryContracts;
using Flashcards.Core.DTO;
using Flashcards.Core.ServiceContracts;
using Flashcards.Core.Services.Comparers;
using Flashcards.Core.Services.Helpers;
using System.Diagnostics;

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

		public async Task<AffectedResponse> DeleteCards(Guid[]? cardIds)
		{
			await ValidationHelper.ValidateObjects(cardIds);

			var deleted = await _repository.DeleteManyAsync(cardIds!);

			return new AffectedResponse { Affected = deleted };
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

			var cardsIdsToDelete = flashcards!.Where(temp => temp.WhetherToDelete == true).Select(temp => temp.CardId);
			var userFlashcards = (await ConvertRequests(userId!.Value, flashcards!)).ToList();

			userFlashcards.RemoveAll(temp => cardsIdsToDelete.Contains(temp.CardId));

			await _repository.DeleteManyAsync(cardsIdsToDelete);

			var localCards = (await _repository.GetAllAsync(temp => temp.UserId == userId!.Value)).ToHashSet();

			var updatedAnyCard = false;

			foreach (var flashcard in userFlashcards)
			{
				if (localCards.Contains(flashcard, new FlashcardIdEqualityComparer()))
				{
					await _repository.UpdateAsync(temp => temp.CardId == flashcard.CardId, flashcard);
					updatedAnyCard = true;
				}
			}

			if (updatedAnyCard)
			{
				localCards = (await _repository.GetAllAsync(temp => temp.UserId == userId!.Value)).ToHashSet();
			}

			var result = localCards.Union(userFlashcards, new FlashcardIdEqualityComparer()).ToHashSet();

			var cardsToCreate = result.Except(localCards, new FlashcardIdEqualityComparer()).ToList();

			if (cardsToCreate.Count != 0)
			{
				await _repository.CreateManyAsync(cardsToCreate);
			}

			return _mapper.Map<IEnumerable<FlashcardResponse>>(result);
		}	

		public async Task<AffectedResponse> SyncCards(Guid? userId, IEnumerable<FlashcardRequest>? flashcards)
		{
			await ValidationHelper.ValidateObjects(flashcards, userId);

			var totallyAffected = 0;

			var cards = await ConvertRequests(userId!.Value, flashcards!);

			var allCards = await _repository.GetAllAsync(temp => temp.UserId == userId);

			foreach (var item in cards)
			{
				var existingCard = allCards.FirstOrDefault(temp => temp.CardId == item.CardId);

				if (existingCard == null)
				{
					await _repository.CreateAsync(item);
					totallyAffected++;
				}
				else
				{
					totallyAffected += await _repository.UpdateAsync(temp => temp.CardId == item.CardId, item);
				}
			}

			var cardsToDelete = allCards.Except(cards, new FlashcardIdEqualityComparer());

			foreach (var card in cardsToDelete)
			{
				totallyAffected += await _repository.DeleteAsync(temp => temp.CardId == card.CardId);
			}

			return new AffectedResponse { Affected = totallyAffected };
		}

		private async Task<IEnumerable<Flashcard>> ConvertRequests(Guid userId, IEnumerable<FlashcardRequest> flashcards)
		{
			return await Task.FromResult(flashcards.Select(temp => new Flashcard()
			{
				CardId = temp.CardId,
				EFactor = temp.EFactor,
				NextRepeatDate = temp.NextRepeatDate,
				MainSide = temp.MainSide,
				OppositeSide = temp.OppositeSide,
				RepetitionCount = temp.RepetitionCount,
				UserId = userId
			}));
		}
	}
}
