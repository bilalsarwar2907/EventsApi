using EventsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
    }
}