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
  [Route("api/[controller]")]
  [ApiController]
  public class GroupMembersController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GroupMembersController(ApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }
    // POST: api/group-members
    [HttpPost]
    // [Authorize]
    public async Task<ActionResult<GroupMember>> CreateGroupMember([FromBody] GroupMemberCreateModel model)
    {
      var group = _mapper.Map<GroupMember>(model);

      _context.GroupMembers.Add(group);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetGroups), new { Id = group.Id }, group);
    }

    // GET: api/group-members
    [HttpGet]
    //[Authorize]
    public async Task<ActionResult<IEnumerable<GroupMember>>> GetGroups(
        string keyword = "",
        int pageIndex = 0,
        int pageSize = 6)
    {
      var query = _context.GroupMembers.AsQueryable();

      if (_context.GroupMembers == null)
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

    // GET: api/group-members/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupMember>> GetGroup(int id)
    {
      if (_context.GroupMembers == null)
      {
        return NotFound();
      }
      var group = await _context.GroupMembers.FindAsync(id);

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
      var group = await _context.GroupMembers.FindAsync(Id);
      if (group == null)
      {
        return NotFound();
      }

      _context.GroupMembers.Remove(group);
      await _context.SaveChangesAsync();

      return NoContent();
    }
    // PUT: api/GroupMembers/5
    [HttpPut("{Id}")]
    //[Authorize]
    public async Task<IActionResult> UpdateGroup(int Id, GroupMemberUpdateModel model)
    {
      if (Id != model.Id)
      {
        return BadRequest();
      }

      var group = _mapper.Map<GroupMember>(model);
      _context.Entry(group).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!GroupExists(Id))
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
    private bool GroupExists(int Id)
    {
      return _context.Groups.Any(p => p.Id == Id);
    }

  }
}
