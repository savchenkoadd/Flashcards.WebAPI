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

		public async Task<int> DeleteAsync(Guid id)
		{
			var filterDefinition = _builder.Eq(temp => temp.CardId, id);

			var result = await _flashcardsCollection.DeleteOneAsync(filterDefinition);

			return (int)result.DeletedCount;
		}

		public async Task<IReadOnlyCollection<Flashcard>> GetAllAsync(Expression<Func<Flashcard, bool>> expression)
		{
			return await _flashcardsCollection.Find(expression).ToListAsync();
		}

		public async Task<int> UpdateAsync(Guid id, Flashcard entity)
		{
			var filterDefinition = _builder.Eq(temp => temp.CardId, id);

			var result = await _flashcardsCollection.ReplaceOneAsync(filterDefinition, entity);

			return (int)result.ModifiedCount;
		}
	}
}
