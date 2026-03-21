using EventsApi.Models;


namespace EventsApi.Repos
{
    public class RepoList<T> :  IRepoList<T> where T :class, IIdable
    {
        private readonly List<T> _items = new();
        private int _nextId = 1;

        public RepoList(bool includeData = false)
        {
            if (includeData && typeof(T) == typeof(Event))
            {
                Add((T)(IIdable)new Event { Title = "Rock Night", Date = new DateTime(2025, 6, 15), Category = "Music", Price = 120 });
                Add((T)(IIdable)new Event { Title = "Tech Summit", Date = new DateTime(2025, 8, 20), Category = "Conference", Price = 500 });
                Add((T)(IIdable)new Event { Title = "Food Festival", Date = new DateTime(2025, 7, 4), Category = "Food", Price = 75 });
            }
        }

        public IEnumerable<T> GetAll(string? category = null, string? sortBy = null, bool descending = false)
        {
            IEnumerable<T> result = _items.AsReadOnly();

            if (category != null && typeof(T) == typeof(Event))
            {
                result = result.Where(item =>
                {
                    var ev = item as Event;
                    return ev?.Category != null &&
                           ev.Category.Contains(category, StringComparison.OrdinalIgnoreCase);
                });
            }

            if (sortBy?.ToLower() == "date" && typeof(T) == typeof(Event))
            {
                result = descending
                    ? result.OrderByDescending(item => (item as Event)!.Date)
                    : result.OrderBy(item => (item as Event)!.Date);
            }
            else if (sortBy?.ToLower() == "price" && typeof(T) == typeof(Event))
            {
                result = descending
                    ? result.OrderByDescending(item => (item as Event)!.Price)
                    : result.OrderBy(item => (item as Event)!.Price);
            }

            return result;
        }

        public T? GetById(int id) =>
            _items.FirstOrDefault(item => item.Id == id);

        public T Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            item.Id = _nextId++;
            _items.Add(item);
            return item;
        }

        public T? Update(int id, T updatedItem)
        {
            var existing = GetById(id);
            if (existing == null) 
                return null ;

            if (existing is Event existingEvent && updatedItem is Event updatedEvent)
            {
                existingEvent.Title = updatedEvent.Title;
                existingEvent.Date = updatedEvent.Date;
                existingEvent.Category = updatedEvent.Category;
                existingEvent.Price = updatedEvent.Price;
            }

            return existing;
        }

        public T? Delete(int id)
        {
            var item = GetById(id);
            if (item != null) _items.Remove(item);
            return item;
        }
    }
}