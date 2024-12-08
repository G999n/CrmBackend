using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerRelationshipManagement.Models;
using System.Threading.Tasks;
using CustomerRelationshipManagement.Data;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public OpportunityController(CrmDbContext context)
        {
            _context = context;
        }

        // POST: api/Opportunity
        [HttpPost]
        public async Task<IActionResult> CreateOpportunity([FromBody] Opportunity opportunity)
        {
            // Check if the Customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == opportunity.CustomerId);
            if (!customerExists)
            {
                return NotFound(new { message = "Customer not found." });
            }

            // Check if the Account Manager exists and has the role "AccManager"
            var accountManagerExists = await _context.Users
                .AnyAsync(u => u.UserId == opportunity.AccountManagerId && u.Role == "AccManager");

            if (!accountManagerExists)
            {
                return NotFound(new { message = "User is not an Account Manager or does not exist." });
            }

            // Add the opportunity to the database
            _context.Opportunities.Add(opportunity);
            await _context.SaveChangesAsync();

            // Create a notification for the account manager upon opportunity creation
            var notification = new Notification
            {
                UserId = opportunity.AccountManagerId,
                Type = "New Opportunity Created",  // Type of the notification
                Message = "A new opportunity has been created for customer ID " + opportunity.CustomerId,
                Timestamp = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOpportunityById), new { id = opportunity.OpportunityId }, opportunity);
        }

        // GET: api/Opportunity/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Opportunity>> GetOpportunityById(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);

            if (opportunity == null)
            {
                return NotFound(new { message = "Opportunity not found." });
            }

            return Ok(opportunity);
        }

        // PUT: api/Opportunity/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpportunity(int id, [FromBody] Opportunity opportunity)
        {
            if (id != opportunity.OpportunityId)
            {
                return BadRequest(new { message = "Opportunity ID mismatch." });
            }

            // Check if the Customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == opportunity.CustomerId);
            if (!customerExists)
            {
                return NotFound(new { message = "Customer not found." });
            }

            // Check if the Account Manager exists and has the role "AccManager"
            var accountManagerExists = await _context.Users
                .AnyAsync(u => u.UserId == opportunity.AccountManagerId && u.Role == "AccManager");

            if (!accountManagerExists)
            {
                return NotFound(new { message = "User is not an Account Manager or does not exist." });
            }

            _context.Entry(opportunity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Create a notification for the account manager upon opportunity update
            var notification = new Notification
            {
                UserId = opportunity.AccountManagerId,
                Type = "Opportunity Updated",  // Type of the notification
                Message = "The opportunity for customer ID " + opportunity.CustomerId + " has been updated.",
                Timestamp = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully updated
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Opportunity>>> GetAllOpportunities()
        {
            var opportunities = await _context.Opportunities.ToListAsync();
            return Ok(opportunities);
        }

        // DELETE: api/Opportunity/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpportunity(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);

            if (opportunity == null)
            {
                return NotFound(new { message = "Opportunity not found." });
            }

            _context.Opportunities.Remove(opportunity);
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully deleted
        }
    }
}
