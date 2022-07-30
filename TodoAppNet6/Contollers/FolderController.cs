using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppNet6.Data;
using TodoAppNet6.Models.Folders;
using TodoAppNet6.Models.TodoItem;
using TodoAppNet6.Services.UserProp;

namespace TodoAppNet6.Contollers
{
    [Route("api/folder")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IUserService _userService;

        public FolderController(TodoContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Folder>>> GetFolder()
        {
            if (_context.Folder == null)
            {
                return NotFound();
            }
            return await _context.Folder.ToListAsync();
        }

        [HttpGet("inside/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Todo>>> GetFoldersTodoes(Guid id)
        {
            if (_context.Todo == null)
                return NotFound();

            if (!FolderExists(id))
                return NotFound("No such a folder");

            var todoes = await _context.Todo
                                .Where(t => t.FolderId == id)
                                .ToListAsync();

            return todoes;
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Folder>> GetFolder(Guid id)
        {
          if (_context.Folder == null)
          {
              return NotFound();
          }
            var folder = await _context.Folder.FindAsync(id);

            if (folder == null)
            {
                return NotFound();
            }

            return folder;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFolder(Guid id, FolderDto request)
        {
            if (!FolderExists(id))
            {
                return BadRequest("No such a folder!");
            }

            var folder = GetFolder(id).Result.Value;
            folder!.Name = request.Name;
            folder!.Description = request.Description;
            if (request.UserId != null)
                folder!.UserId = request.UserId;

            _context.Entry(folder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FolderExists(id))
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Folder>> PostFolder(FolderDto request)
        {
            if (_context.Folder == null)
            {
                return Problem("Entity set 'TodoContext.Folder'  is null.");
            }

            var userId = request.UserId;
            if (userId == null)
                userId = _userService.GetId();

            var user = _userService.GetUserById(userId!);

            var folder = new Folder
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                UserId = request.UserId,
                User = user,
                Todoes = new List<Todo> { }
            };

            _context.Folder.Add(folder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFolder", new { id = folder.Id }, folder);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFolder(Guid id)
        {
            if (_context.Folder == null)
            {
                return NotFound();
            }
            var folder = await _context.Folder.FindAsync(id);
            if (folder == null)
            {
                return NotFound();
            }

            _context.Folder.Remove(folder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FolderExists(Guid id)
        {
            return (_context.Folder?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
