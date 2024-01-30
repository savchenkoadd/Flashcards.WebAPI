using Flashcards.Core.Domain.Identity;
using Flashcards.Core.DTO.JWT;

namespace Flashcards.Core.ServiceContracts
{
    public interface IJwtService
    {
        Task<AuthenticationResponse> CreateJwtToken(ApplicationUser applicationUser);
    }
}
