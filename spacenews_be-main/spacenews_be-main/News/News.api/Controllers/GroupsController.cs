using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.api.Data;
using News.api.Entities;
using News.api.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace News.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GroupsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //POST : api/posts/add_group
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] GroupCreateModel model)
        {
            var data = _mapper.Map<Group>(model);

            _context.Groups.Add(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroups), new { Id = data.Id }, data);
        }

        //GET : Api/posts/get_groups
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups(
            string keyword = "",
            int pageIndex = 0,
            int pageSize = 6)
        {
            var query = _context.Groups.AsQueryable();

            if (_context.Groups == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(s => s.Name.Contains(keyword));


            var result = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToListAsync();
            return result;
        }

        //GET: Api/Groups/5
        [HttpGet("{Id}")]
        public async Task<ActionResult<Group>> GetGroup(int Id)
        {
            var group = await _context.Groups.FindAsync(Id)
;

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }
        // DELETE: api/Groups/5
        [HttpDelete("{Id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteGroup(int Id)
        {
            var group = await _context.Groups.FindAsync(Id)
;
            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int Id)
        {
            return _context.Groups.Any(p => p.Id == Id);
        }

    }
}
