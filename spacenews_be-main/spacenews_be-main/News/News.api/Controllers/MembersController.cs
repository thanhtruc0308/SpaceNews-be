using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using News.api.Data;
using News.api.Entities;
using News.api.ViewModels;

namespace News.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public MembersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //POST : api/members/add_members
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<Member>> CreateMember([FromBody] MemberCreateModel model)
        {
            var member = _mapper.Map<Member>(model);

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembers), new { id = member.Id }, member);
        }

        //GET : Api/members/get_nembers
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            return await _context.Members.ToListAsync();
        }
        // GET: api/members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(id)
;

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }
        //PUT: api/members/put
        [HttpPut("{Id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateMember(int Id, MemberUpdateModel model)
        {
            if (Id != model.Id)
            {
                return BadRequest();
            }

            var member = _mapper.Map<Member>(model);
            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(Id))
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
        // DELETE: api/members/5
        [HttpDelete("{Id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteMember(int Id)
        {
            var member = await _context.Members.FindAsync(Id)
;
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool MemberExists(int Id)
        {
            return _context.Members.Any(p => p.Id == Id);
        }

    }

}