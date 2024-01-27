using Flashcards.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Db
{
	public class AccountDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
	{
        public AccountDbContext(DbContextOptions options) : base(options)
        {
        }

        public AccountDbContext()
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
