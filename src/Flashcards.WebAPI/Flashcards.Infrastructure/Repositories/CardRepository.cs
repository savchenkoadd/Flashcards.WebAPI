using Flashcards.Core.Domain.Entities;
using Flashcards.Core.Domain.RepositoryContracts;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Flashcards.Infrastructure.Repositories
{
	public class CardRepository : IRepository<Flashcard>
	{
		private const string COLLECTION_NAME = "flashcards";

		private readonly IMongoCollection<Flashcard> _flashcardsCollection;
		private readonly FilterDefinitionBuilder<Flashcard> _builder = Builders<Flashcard>.Filter;

		public CardRepository(
				IMongoDatabase mongoDatabase
			)
		{
			_flashcardsCollection = mongoDatabase.GetCollection<Flashcard>(COLLECTION_NAME);
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
			var found = await _flashcardsCollection.FindAsync(expression);

			return found.ToEnumerable();
		}

		public async Task<int> UpdateAsync(Expression<Func<Flashcard, bool>> expression, Flashcard card)
		{
			var found = (await GetAllAsync(temp => temp.CardId == card.CardId)).First();

			card._id = found._id;

			if (card.Equals(found))
			{
				return 0;
			}

			var result = await _flashcardsCollection.ReplaceOneAsync(expression, card);

			return (int)result.ModifiedCount;
		}

		public async Task<long> Count(Expression<Func<Flashcard, bool>> expression)
		{
			return await _flashcardsCollection.CountDocumentsAsync(expression);
		}

		public async Task CreateManyAsync(IEnumerable<Flashcard> entities)
		{
			await _flashcardsCollection.InsertManyAsync(entities);
		}

		public async Task<long> DeleteManyAsync(IEnumerable<Guid> guids)
		{
			var filter = _builder.In(card => card.CardId, guids);
			var result = await _flashcardsCollection.DeleteManyAsync(filter);

			return result.DeletedCount;
		}
	}
}
