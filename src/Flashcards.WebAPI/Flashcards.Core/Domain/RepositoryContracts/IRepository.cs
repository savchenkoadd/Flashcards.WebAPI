using System.Linq.Expressions;

namespace Flashcards.Core.Domain.RepositoryContracts
{
	public interface IRepository<T>
	{
		Task<int> UpdateAsync(Expression<Func<T, bool>> expression, T entity);

		Task CreateAsync(T entity);

		Task<int> DeleteAsync(Expression<Func<T, bool>> expression);

		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);

		Task<long> Count(Expression<Func<T, bool>> expression);
	}
}
