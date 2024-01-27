using Flashcards.Core.Domain.Entities;
using Flashcards.Core.Domain.RepositoryContracts;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Flashcards.Infrastructure.Repositories
{
	public class CardRepository : IRepository<Flashcard>
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

        public async Task CreateAsync(Flashcard entity)
		{
			await _flashcardsCollection.InsertOneAsync(entity);
		}

		public async Task<int> DeleteAsync(Expression<Func<Flashcard, bool>> expression)
		{
			var result = await _flashcardsCollection.DeleteOneAsync(expression);

			return (int)result.DeletedCount;
		}

		public async Task<IEnumerable<Flashcard>> GetAllAsync(Expression<Func<Flashcard, bool>> expression)
		{
			return await _flashcardsCollection.Find(expression).ToListAsync();
		}

		public async Task<int> UpdateAsync(Expression<Func<Flashcard, bool>> expression, Flashcard entity)
		{
			var result = await _flashcardsCollection.ReplaceOneAsync(expression, entity);

			return (int)result.ModifiedCount;
		}
	}
}
