﻿using Flashcards.Core.Domain.Identity;
using Flashcards.Core.DTO;
using Flashcards.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.WebAPI.Controllers
{
	[Route("api")]
	[ApiController]
	public class CardsController : ControllerBase
	{
		private readonly ICardService _cardService;
		private readonly UserManager<ApplicationUser> _userManager;

        public CardsController(
				ICardService cardService,
				UserManager<ApplicationUser> userManager
			)
        {
            _cardService = cardService;
			_userManager = userManager;
        }

		//GET: /api/GetAllCards
		/// <summary>
		/// Retrieves all cards related to a specific user.
		/// The property "nextRepeatDate" is provided in the following format: yyyy-MM-dd.
		/// To use this endpoint, you must be logged in.
		/// </summary>
		/// <returns>List of retrieved flashcards</returns>
		[HttpGet("[action]")]
		public async Task<ActionResult<IEnumerable<FlashcardResponse>>> GetAllCards()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Problem(detail: "You must be logged in to use this endpoint.", statusCode: 400);
			}

			var userId = (await _userManager.GetUserAsync(User)).Id;

			var cards = await _cardService.GetAllAsync(userId);

			return cards.ToList();
		}

		//POST: /api/SyncCards
		/// <summary>
		/// Currently obsolete.
		/// Synchronizes received cards with the cards in the storage. 
		/// Cards which do not exist in the provided list, but exist in the storage will be permanently removed from the storage.
		/// Cards which exist in the provided list, but do not exist in the storage will be automatically created in the storage.
		/// Changed cards will be automatically updated in the storage.
		/// You should provide "nextRepeatDate" property in the following format: yyyy-MM-dd.
		/// To use this endpoint, you must be logged in.
		/// </summary>
		/// <param name="flashcards">List of cards.</param>
		/// <returns>Number of affected rows after synchronizing.</returns>
		[HttpPost("[action]")]
        public async Task<ActionResult<AffectedResponse>> SyncCards(List<FlashcardRequest>? flashcards)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Problem(detail: "You must be logged in to use this endpoint.", statusCode: 400);
			}

			var user = (await _userManager.GetUserAsync(User));

			return await _cardService.SyncCards(user.Id, flashcards);
		}

		//POST: /api/DeleteCards
		/// <summary>
		/// Deletes cards in the storage with the provided ids.
		/// </summary>
		/// <param name="cardsIds">Ids of cards to be deleted</param>
		/// <returns>Count of deleted rows</returns>
		[HttpPost("[action]")]
		public async Task<ActionResult<AffectedResponse>> DeleteCards(Guid[]? cardsIds)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Problem(detail: "You must be logged in to use this endpoint.", statusCode: 400);
			}

			return await _cardService.DeleteCards(cardsIds);
		}

		//POST: /api/SyncAndGetCards
		[HttpPost("[action]")]
		public async Task<ActionResult<IEnumerable<FlashcardResponse>>> SyncAndGetCards(IEnumerable<FlashcardRequest> flashcards)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Problem(detail: "You must be logged in to use this endpoint.", statusCode: 400);
			}

			var user = (await _userManager.GetUserAsync(User));

			return (await _cardService.SyncAndGetCards(user.Id, flashcards)).ToList();
		}
	}
}
