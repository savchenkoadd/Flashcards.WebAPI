using System.Linq.Expressions;

namespace Flashcards.Core.Domain.RepositoryContracts
{
	public interface IRepository<T>
	{
		Task<int> UpdateAsync(Guid id, T entity);

		Task CreateAsync(T entity);

		Task<int> DeleteAsync(Expression<Func<T, bool>> expression);

		Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> expression);
	}
}
