using CustomerRelationshipManagement.Data;
using CustomerRelationshipManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public LeadController(CrmDbContext context)
        {
            _context = context;
        }

        // GET: api/Lead
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lead>>> GetLeads()
        {
            return await _context.Leads.ToListAsync();
        }

        // GET: api/Lead/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Lead>> GetLead(int id)
        {
            var lead = await _context.Leads.FindAsync(id);

            if (lead == null)
            {
                return NotFound();
            }

            return lead;
        }

        // POST: api/Lead
        [HttpPost]
        public async Task<ActionResult<Lead>> CreateLead(Lead lead)
        {
            // Validate AssignedTo (UserId) exists in Users DB
            var userExists = await _context.Users.AnyAsync(u => u.UserId == lead.AssignedTo);
            if (!userExists)
            {
                return BadRequest($"User with ID {lead.AssignedTo} does not exist.");
            }

            // Add the lead to the database
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            // Create a notification for the assigned user
            var notification = new Notification
            {
                Type = "New Lead Assigned",
                Message = $"A new lead (ID: {lead.LeadId}) has been assigned to you.",
                Timestamp = DateTime.UtcNow,
                UserId = lead.AssignedTo
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLead), new { id = lead.LeadId }, lead);
        }

        // PUT: api/Lead/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLead(int id, Lead lead)
        {
            if (id != lead.LeadId)
            {
                return BadRequest();
            }

            // Validate AssignedTo (UserId) exists in Users DB
            var userExists = await _context.Users.AnyAsync(u => u.UserId == lead.AssignedTo);
            if (!userExists)
            {
                return BadRequest($"User with ID {lead.AssignedTo} does not exist.");
            }

            _context.Entry(lead).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeadExists(id))
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

        // POST: api/Lead/Convert/{id}
        [HttpPost("Convert/{id}")]
        public async Task<IActionResult> ConvertLead(int id)
        {
            var lead = await _context.Leads.FindAsync(id);

            if (lead == null)
            {
                return NotFound(new { message = "Lead not found." });
            }

            // Simulate conversion logic: Mark lead as converted
            lead.IsConverted = true;  // Assume there is an IsConverted field in the Lead model
            _context.Entry(lead).State = EntityState.Modified;

            // Notify the assigned user about the lead conversion
            var notification = new Notification
            {
                Type = "Lead Conversion",
                Message = $"Lead '{lead.Name}' has been successfully converted to a customer.",
                Timestamp = DateTime.Now,
                UserId = lead.AssignedTo,
                IsRead = false
            };

            _context.Notifications.Add(notification);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Lead converted successfully and notification sent." });
        }

        // DELETE: api/Lead/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeadExists(int id)
        {
            return _context.Leads.Any(e => e.LeadId == id);
        }
    }
}
