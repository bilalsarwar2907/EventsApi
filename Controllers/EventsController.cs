using EventsApi.Models;
using EventsApi.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IRepoList<Event> _repo;

        public EventsController(IRepoList<Event> repo)
        {
            _repo = repo;
        }

        // GET /events
        // GET /events?category=Music
        // GET /events?sortBy=price&descending=true
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] string? category,
            [FromQuery] string? sortBy,
            [FromQuery] bool descending = false)
        {
            var events = _repo.GetAll(category, sortBy, descending);
            return Ok(events);
        }

        // GET /events/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var ev = _repo.GetById(id);
            if (ev == null) return NotFound();
            return Ok(ev);
        }

        // POST /events
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromBody] Event ev)
        {
            var created = _repo.Add(ev);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /events/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Event updatedEvent)
        {
            var result = _repo.Update(id, updatedEvent);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE /events/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _repo.Delete(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
