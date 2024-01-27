using Flashcards.Core.Domain.Entities;
using Flashcards.Core.Domain.RepositoryContracts;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Flashcards.Infrastructure.Repositories
{
	public class CardRepository : IRepository<Flashcard>, ICardRepository
	{
		private readonly IMongoCollection<Flashcard> _flashcardsCollection;
		private readonly FilterDefinitionBuilder<Flashcard> _builder;

        public CardRepository(
				IMongoDatabase mongoDatabase, string collectionName
			)
        {
			_flashcardsCollection = mongoDatabase.GetCollection<Flashcard>(collectionName);
			_builder = Builders<Flashcard>.Filter;
        }

        public Task CreateAsync(Flashcard entity)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyCollection<Flashcard>> GetAllAsync(Expression<Func<Flashcard, bool>> expression)
		{
			throw new NotImplementedException();
		}

		public Task<int> SyncCards(List<Flashcard> flashcards)
		{
			throw new NotImplementedException();
		}

		public Task<int> UpdateAsync(Guid id, Flashcard entity)
		{
			throw new NotImplementedException();
		}
	}
}
