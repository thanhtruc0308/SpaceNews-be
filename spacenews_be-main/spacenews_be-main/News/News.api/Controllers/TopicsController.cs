using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.api.Data;
using News.api.Entities;
using News.api.ViewModels;

namespace News.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper  _mapper;

        public TopicsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Topics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
        {
          if (_context.Topics == null)
          {
              return NotFound();
          }
            return await _context.Topics.Include(s => s.Posts).ToListAsync();
        }

        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopic(int id)
        {
          if (_context.Topics == null)
          {
              return NotFound();
          }
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
            {
                return NotFound();
            }

            return topic;
        }

        // PUT: api/Topics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTopic(int id, TopicUpdateModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var topic = _mapper.Map<Topic>(model);
            _context.Entry(topic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Topics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Topic>> PostTopic(TopicCreateModel model)
        {
          if (_context.Topics == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Topics'  is null.");
          }
          var topic = _mapper.Map<Topic>(model);
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            CreatedAtAction("GetTopic", new { id = topic.Id }, topic);
            return topic;
        }

        // DELETE: api/Topics/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            if (_context.Topics == null)
            {
                return NotFound();
            }
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TopicExists(int id)
        {
            return (_context.Topics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
