using Microsoft.AspNetCore.Identity;

namespace Flashcards.Core.Domain.Identity
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string? PersonName { get; set; }
	}
}
