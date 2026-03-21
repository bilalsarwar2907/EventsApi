namespace EventsApi.Models
{
    public class Event : IIdable
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"Event Id: {Id}, Title: {Title}, Date: {Date:yyyy-MM-dd}, Category: {Category}, Price: {Price}";
        }
    }
}