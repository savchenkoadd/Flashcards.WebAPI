using AutoMapper;
using Flashcards.Core.Domain.Entities;
using Flashcards.Core.DTO;

namespace Flashcards.WebAPI.Profiles
{
	public class CommonMapperProfile : Profile
	{
        public CommonMapperProfile()
        {
            CreateMap<FlashcardRequest, Flashcard>();
			CreateMap<Flashcard, FlashcardResponse>();
		}
    }
}
