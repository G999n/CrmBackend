using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerRelationshipManagement.Models;
using System.Threading.Tasks;
using CustomerRelationshipManagement.Data;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public TaskController(CrmDbContext context)
        {
            _context = context;
        }

        // POST: api/Task
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Models.Task task)
        {
            // Check if the Customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == task.CustomerId);
            if (!customerExists)
            {
                return NotFound(new { message = "Customer not found." });
            }

            // Check if the User (AssignedTo) exists
            var userExists = await _context.Users.AnyAsync(u => u.UserId == task.AssignedTo);
            if (!userExists)
            {
                return NotFound(new { message = "User (AssignedTo) not found." });
            }

            // Add the task to the database
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Create a notification for the user assigned to the task
            var notification = new Notification
            {
                UserId = task.AssignedTo,  // The user who is assigned to the task
                Type = "New Task Assigned",
                Message = $"A new task has been assigned to you: {task.TaskDescription}.",
                Timestamp = DateTime.UtcNow
            };

            // Add the notification to the database
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Return the created task
            return CreatedAtAction(nameof(GetTaskById), new { id = task.TaskId }, task);
        }

        // GET: api/Task/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTaskById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            return Ok(task);
        }

        [HttpGet()]
        public async Task<ActionResult<Models.Task>> GetAllTasks(int id)
        {
            var task = await _context.Tasks.ToListAsync();
            return Ok(task);
        }

        // PUT: api/Task/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Models.Task task)
        {
            if (id != task.TaskId)
            {
                return BadRequest(new { message = "Task ID mismatch." });
            }

            // Check if the Customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == task.CustomerId);
            if (!customerExists)
            {
                return NotFound(new { message = "Customer not found." });
            }

            // Check if the User (AssignedTo) exists
            var userExists = await _context.Users.AnyAsync(u => u.UserId == task.AssignedTo);
            if (!userExists)
            {
                return NotFound(new { message = "User (AssignedTo) not found." });
            }

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully updated
        }

        // DELETE: api/Task/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully deleted
        }
    }
}
