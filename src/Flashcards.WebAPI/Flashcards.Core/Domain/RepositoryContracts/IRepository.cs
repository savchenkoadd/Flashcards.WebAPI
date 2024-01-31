using System.Linq.Expressions;

namespace Flashcards.Core.Domain.RepositoryContracts
{
	public interface IRepository<T>
	{
		Task<int> UpdateAsync(Expression<Func<T, bool>> expression, T entity);

		Task CreateAsync(T entity);

		Task CreateManyAsync(IEnumerable<T> entities);

		Task<int> DeleteAsync(Expression<Func<T, bool>> expression);

		Task<long> DeleteManyAsync(IEnumerable<Guid> guids);

		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);

		Task<long> Count(Expression<Func<T, bool>> expression);
	}
}
