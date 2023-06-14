using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using News.api.Data;
using News.api.Entities;
using News.api.ViewModels;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.Diagnostics.Metrics;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace News.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PostsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts(
            int? topicId = null,
            string keyword = "",
            string groupId = null,//
            int pageIndex = 0,
            int pageSize = 10,
            bool? showSlider = null,
            bool? previousTime = null,
            bool? ascendingOrder = null
            )
        {
            DateTime currentDay = DateTime.UtcNow.Date;
            var query = _context.Posts.Include(s => s.Topic).AsQueryable();

            if (topicId != null)
            {
                query = query.Where(s => s.TopicID == topicId);
            }
            //
            if (groupId != null)
            {
                query = query.Where(s => s.GroupID.Contains(groupId));
            }
            //

            if (showSlider == true)
            {
                query = query.Where(s => s.ShowInSlider == true);
            }
            else if (showSlider != null & showSlider == false)
            {
                query = query.Where(s => s.ShowInSlider == false);
            }

            if (ascendingOrder != null)
            {
                query = query.OrderBy(s => s.Date);
            }
            if (previousTime == false)
            {
                query = query.Where(s => s.Date >= currentDay);
            }
            else if (previousTime == true && previousTime != null)
            {
                query = query.Where(s => s.Date < currentDay);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(s => s.Content.Contains(keyword)
                || (s.Topic != null && s.Topic.Name.Contains(keyword)) || (s.Title.Contains(keyword))
                );

            var result = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToListAsync();
            return result;
        }

        // GET: api/posts/5
        [HttpGet("{Id}")]
        public async Task<ActionResult<Post>> GetPost(int Id)
        {
            var post = await _context.Posts.FindAsync(Id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // GET : api/posts/false // count post
        [HttpGet("Quantity")]
        public async Task<ActionResult<int>> CountPosts(bool? previousTime = false)
        {
            DateTime currentDay = DateTime.UtcNow.Date;
            var query = _context.Posts.AsQueryable();
            if (previousTime == false && previousTime != null)
            {
                query = query.Where(s => s.Date >= currentDay);
            }
            else query = query.Where(s => s.Date < currentDay);
            var result = query.Count();
            return result;
        }



        // POST: api/posts
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateModel model)
        {
            var haveUpdated = true;
            var transaction = await _context.Database.BeginTransactionAsync();
            var post = _mapper.Map<Post>(model);

            _context.Posts.Add(post);

            if (post.GroupID == "")
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return CreatedAtAction(nameof(GetPost), new { Id = post.Id }, post);
            }
            else
            {
                var existingGroupIdList = _context.Groups.Select(s => s.Id).ToList();
                if (existingGroupIdList == null)
                {
                    return NotFound();
                }
                else
                {
                    post.GroupID = string.Join(",", post.GroupID.Split(',').Select(s => s.Trim()).ToArray()).TrimEnd(',');
                }
                int[] groupIds = post.GroupID.Split(',').Select(int.Parse).ToArray();
                try
                {
                    foreach (int groupId in groupIds)
                    {
                        if (!existingGroupIdList.Contains(groupId))
                        {
                            haveUpdated = false;
                        }
                    }
                    if (haveUpdated)
                    {
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return CreatedAtAction(nameof(GetPost), new { Id = post.Id }, post);
                    }

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }

                return NotFound("GroupId field not being existed!");
            }

        }

        // PUT: api/posts/5
        [HttpPut("{Id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int Id, PostUpdateModel model)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            var haveUpdated = true;
            var post = _mapper.Map<Post>(model);
            post.GroupID = string.Join(",", post.GroupID.Split(',').Select(s => s.Trim()).ToArray()).TrimEnd(',');
            if (Id != model.Id)
            {
                return BadRequest();
            }
            if (post.GroupID == "")
            {
                _context.Entry(post).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return NoContent();
            }
            else
            {
                var existingGroupIdList = _context.Groups.Select(s => s.Id).ToList();

                int[] groupIds = post.GroupID.Split(',').Select(int.Parse).ToArray();

                try
                {
                    _context.Entry(post).State = EntityState.Modified;

                    foreach (int groupId in groupIds)
                    {
                        if (!existingGroupIdList.Contains(groupId) || !PostExists(Id))
                        {
                            haveUpdated = false;
                        }
                    }
                    if (haveUpdated)
                    {
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return NoContent();
                    }

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                return NotFound("GroupID or PostId not being existed!");
            }
        }


        // DELETE: api/posts/5
        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int Id)
        {
            var post = await _context.Posts.FindAsync(Id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int Id)
        {
            return _context.Posts.Any(p => p.Id == Id);
        }

        private bool GroupExists(int[] id)
        {
            foreach (int groupId in id)
            {
                return _context.Groups.Any(g => g.Id == groupId);
            }
            return true;
        }




    }
}