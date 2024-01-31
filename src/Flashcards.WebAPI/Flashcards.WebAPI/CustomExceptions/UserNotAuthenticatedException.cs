namespace Flashcards.WebAPI.CustomExceptions
{
	public class UserNotAuthenticatedException : Exception
	{
        public UserNotAuthenticatedException() { }

        public UserNotAuthenticatedException(string message) : base(message) { }

		public UserNotAuthenticatedException(string message, Exception inner)
		: base(message, inner) { }
	}
}
