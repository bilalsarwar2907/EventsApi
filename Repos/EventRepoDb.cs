using EventsApi.Data;
using EventsApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventsApi.Repos
{
    public class RepoDB<T> : IRepoList<T> where T : class, IIdable
    {
        private readonly EventDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepoDB(EventDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll(string? category = null, string? sortBy = null, bool descending = false)
        {
            IEnumerable<T> result = _dbSet.AsNoTracking().ToList();

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

        public T? GetById(int id) => _dbSet.Find(id);

        public T Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _dbSet.Add(item);
            _context.SaveChanges();
            return item;
        }

        public T? Update(int id, T updatedItem)
        {
            var existing = GetById(id);
            if (existing == null) return null;

            if (existing is Event existingEvent && updatedItem is Event updatedEvent)
            {
                existingEvent.Title = updatedEvent.Title;
                existingEvent.Date = updatedEvent.Date;
                existingEvent.Category = updatedEvent.Category;
                existingEvent.Price = updatedEvent.Price;
                _context.SaveChanges();
            }

            return existing;
        }

        public T? Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _dbSet.Remove(item);
                _context.SaveChanges();
            }
            return item;
        }
    }
}
