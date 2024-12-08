using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerRelationshipManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerRelationshipManagement.Data;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public NotificationController(CrmDbContext context)
        {
            _context = context;
        }

        // GET: api/Notification/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetUserNotifications(int userId)
        {
            var notifications = await (from n in _context.Notifications
                                       where n.UserId == userId
                                       orderby n.Timestamp descending
                                       select n).ToListAsync();

            return Ok(notifications);
        }

        // POST: api/Notification
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.NotificationId }, notification);
        }

        // GET: api/Notification/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotificationById(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return NotFound(new { message = "Notification not found." });
            }

            return Ok(notification);
        }

        // PUT: api/Notification/MarkAsRead/{id}
        [HttpPut("MarkAsRead/{id}")]
        public async Task<IActionResult> MarkNotificationAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return NotFound(new { message = "Notification not found." });
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Notification/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return NotFound(new { message = "Notification not found." });
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
