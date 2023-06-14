using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.api.Data;
using News.api.Entities;
using Microsoft.AspNetCore.Authorization;
using News.api.ViewModels;

namespace News.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        

        public ReadHistoryController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/readhistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadHistory>>> GetReadHistory()
        {
            if (_context.ReadHistory == null)
            {
                return NotFound();
            }
            return await _context.ReadHistory.ToListAsync();
        }

        [HttpGet("{UserID}")]
        public async Task<ActionResult<ReadHistory>> GetHistory(string UserID)
        {
            if (_context.ReadHistory == null)
            {
                return NotFound();
            }

           
            var data = await _context.ReadHistory.FindAsync(UserID);

            if (data == null)
            {
                return NotFound();
            }

            return data;
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<ReadHistory>> AddHistory(ReadHistoryCreate model)
        {
            if (_context.ReadHistory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ReadHistory'  is null.");
            }


            var data = _mapper.Map<ReadHistory>(model);
            _context.ReadHistory.Add(data);
            await _context.SaveChangesAsync();

            CreatedAtAction("GetHistory", new { UserID = data.UserID }, data);
            return data;
        }

        [HttpPut("{UserID}")]
        //[Authorize]
        public async Task<IActionResult> PutHistories(string UserID, ReadHistoryUpdate model)
        {
            if (UserID != model.UserID)
            {
                return BadRequest();
            }

            var data = _mapper.Map<ReadHistory>(model);
            _context.Entry(data).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return NoContent();
        }

    }
}
