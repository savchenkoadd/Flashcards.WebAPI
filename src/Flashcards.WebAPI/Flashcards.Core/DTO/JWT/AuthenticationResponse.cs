namespace Flashcards.Core.DTO.JWT
{
    public class AuthenticationResponse
    {
        public string? PersonName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
