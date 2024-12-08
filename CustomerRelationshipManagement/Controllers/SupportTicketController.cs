using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerRelationshipManagement.Models;
using System.Threading.Tasks;
using CustomerRelationshipManagement.Data;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportTicketController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public SupportTicketController(CrmDbContext context)
        {
            _context = context;
        }

        // POST: api/SupportTicket
        [HttpPost]
        public async Task<IActionResult> CreateSupportTicket([FromBody] SupportTicket supportTicket)
        {
            // Check if the Customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == supportTicket.CustomerId);
            if (!customerExists)
            {
                return NotFound(new { message = "Customer not found." });
            }

            // Check if the Assigned user exists and has the role "CustSupport"
            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == supportTicket.AssignedTo && u.Role == "CustSupport");

            if (!userExists)
            {
                return NotFound(new { message = "User is not a CustSupport or does not exist." });
            }

            // Add the support ticket to the database
            _context.SupportTickets.Add(supportTicket);
            await _context.SaveChangesAsync();

            // Create a notification for the user assigned to the support ticket
            var notification = new Notification
            {
                UserId = supportTicket.AssignedTo,  // User to whom the ticket is assigned
                Type = "New Support Ticket",  // Type of the notification
                Message = $"A new support ticket has been assigned to you for customer ID {supportTicket.CustomerId}.",
                Timestamp = DateTime.UtcNow  // Current timestamp
            };

            // Add the notification to the database
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupportTicketById), new { id = supportTicket.TicketId }, supportTicket);
        }

        // GET: api/SupportTicket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SupportTicket>> GetSupportTicketById(int id)
        {
            var supportTicket = await _context.SupportTickets.FindAsync(id);

            if (supportTicket == null)
            {
                return NotFound(new { message = "Support Ticket not found." });
            }

            return Ok(supportTicket);
        }

        [HttpGet]
        public async Task<ActionResult<SupportTicket>> GetAllSupportTickets(int id)
        {
            var supportTicket = await _context.SupportTickets.ToListAsync();
            return Ok(supportTicket);
        }

        // PUT: api/SupportTicket/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupportTicket(int id, [FromBody] SupportTicket supportTicket)
        {
            if (id != supportTicket.TicketId)
            {
                return BadRequest(new { message = "Ticket ID mismatch." });
            }

            // Check if the Customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == supportTicket.CustomerId);
            if (!customerExists)
            {
                return NotFound(new { message = "Customer not found." });
            }

            // Check if the Assigned user exists and has the role "CustSupport"
            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == supportTicket.AssignedTo && u.Role == "CustSupport");

            if (!userExists)
            {
                return NotFound(new { message = "User is not a CustSupport or does not exist." });
            }

            _context.Entry(supportTicket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully updated
        }

        // DELETE: api/SupportTicket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupportTicket(int id)
        {
            var supportTicket = await _context.SupportTickets.FindAsync(id);

            if (supportTicket == null)
            {
                return NotFound(new { message = "Support Ticket not found." });
            }

            _context.SupportTickets.Remove(supportTicket);
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully deleted
        }
    }
}
