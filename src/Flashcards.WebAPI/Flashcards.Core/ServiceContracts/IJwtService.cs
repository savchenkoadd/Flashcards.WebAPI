using Flashcards.Core.Domain.Identity;
using Flashcards.Core.DTO.JWT;

namespace Flashcards.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser applicationUser);
    }
}
