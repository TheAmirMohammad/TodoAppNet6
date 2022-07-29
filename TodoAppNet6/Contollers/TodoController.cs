using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppNet6.Data;
using TodoAppNet6.Models.TodoItem;
using TodoAppNet6.Services.UserProp;

namespace TodoAppNet6.Contollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IUserInterface _userService;

        public TodoController(TodoContext context, IUserInterface userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodo()
        {
            if (_context.Todo == null)
                return NotFound();

            return await _context.Todo.ToListAsync();
        }

        [HttpGet("mine")]
        [Authorize]
        public async Task<ActionResult<List<Todo>>> GetUsersTodoes()
        {
            if (_context.Todo == null)
                return NotFound();

            var userId = _userService.GetId();
            var todoes = await _context.Todo
                               .Where(t => t.UserId == userId)
                               .ToListAsync();

            return todoes;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Todo>> GetTodo(Guid id)
        {
            if (_context.Todo == null)
                return NotFound();

            var todo = await _context.Todo.FindAsync(id);

            if (todo == null)
                return NotFound();

            return todo;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Todo>> PutTodo(Guid id, TodoDto request)
        {
            if (!TodoExists(id))
                return BadRequest("No such an item!");

            var todo = GetTodo(id).Result.Value;
            todo!.Title = request.Title;
            todo!.Description = request.Description;
            todo!.IsDone = request.IsDone;

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return todo;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Todo>> PostTodo(TodoDto request)
        {
            if (_context.Todo == null)
                return Problem("Entity set 'TodoContext.Todo'  is null.");

            var userId = request.UserId;
            if (userId == null)
                userId = _userService.GetId();

            var user = _userService.GetUserById(userId!);

            var todo = new Todo
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                IsDone = request.IsDone,
                UserId = userId,
                User = user
            };

            _context.Todo.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            if (_context.Todo == null)
                return NotFound();

            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
                return NotFound();

            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(Guid id)
        {
            return (_context.Todo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
