using Flashcards.WebAPI.CustomExceptions;
using System.Security.Claims;

namespace Flashcards.WebAPI.Extensions
{
	public static class UserExtensions
	{
		public async static Task EnsureIsAuthenticated(this ClaimsPrincipal? claimsPrincipal)
		{
			if (claimsPrincipal is null || !claimsPrincipal.Identity.IsAuthenticated)
			{
				throw new UserNotAuthenticatedException("You must be logged in to use this endpoint.");
			}

			await Task.CompletedTask;
		}
	}
}
