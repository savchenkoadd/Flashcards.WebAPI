using Flashcards.Core.Domain.Identity;
using Flashcards.Core.DTO;
using Flashcards.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
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
		[HttpGet("[action]")]
		public async Task<ActionResult<IEnumerable<FlashcardResponse>>> GetAllCards()
		{
			var userId = (await _userManager.GetUserAsync(User)).Id;

			var cards = await _cardService.GetAllAsync(userId);

			return cards;
		}

		//POST: /api/SyncCards
		[HttpPost("[action]")]
        public async Task<ActionResult<AffectedResponse>> SyncCards(List<FlashcardRequest>? flashcards)
		{
			var user = (await _userManager.GetUserAsync(User));

			return await _cardService.SyncCards(user.Id, flashcards);
		}
	}
}
