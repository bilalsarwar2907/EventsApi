using EventsApi.Models;

namespace EventsApi.Repos
{
    public interface IRepoList<T> where T : IIdable
    {
        T Add(T item);
        T? Delete(int id);
        IEnumerable<T> GetAll(string? category = null, string? sortBy = null, bool descending = false);
        T? GetById(int id);
        T? Update(int id, T updatedItem);
    }
}