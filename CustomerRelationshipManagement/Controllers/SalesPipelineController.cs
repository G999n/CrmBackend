using CustomerRelationshipManagement.Data;
using CustomerRelationshipManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPipelineController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public SalesPipelineController(CrmDbContext context)
        {
            _context = context;
        }

        // GET: api/SalesPipeline
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesPipeline>>> GetSalesPipelines()
        {
            return await _context.SalesPipelines.ToListAsync();
        }

        // GET: api/SalesPipeline/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesPipeline>> GetSalesPipeline(int id)
        {
            var salesPipeline = await _context.SalesPipelines.FindAsync(id);

            if (salesPipeline == null)
            {
                return NotFound();
            }

            return salesPipeline;
        }

        // POST: api/SalesPipeline
        [HttpPost]
        public async Task<ActionResult<SalesPipeline>> CreateSalesPipeline(SalesPipeline salesPipeline)
        {
            // Check if the Lead exists
            var lead = await _context.Leads.FirstOrDefaultAsync(l => l.LeadId == salesPipeline.LeadId);
            if (lead == null)
            {
                return BadRequest($"Lead with ID {salesPipeline.LeadId} does not exist.");
            }

            // Check if the Assigned User exists and is not deleted
            var userExists = await _context.Users.AnyAsync(u => u.UserId == lead.AssignedTo);
            if (!userExists)
            {
                return BadRequest($"User with ID {lead.AssignedTo} does not exist or is deleted.");
            }

            _context.SalesPipelines.Add(salesPipeline);
            await _context.SaveChangesAsync();

            // Send a notification for the SalesPipeline creation
            var notification = new Notification
            {
                UserId = lead.AssignedTo,
                Type = "SalesPipeline Created",
                Message = $"A new SalesPipeline has been created for Lead {salesPipeline.LeadId}.",
                Timestamp = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalesPipeline), new { id = salesPipeline.PipelineId }, salesPipeline);
        }

        // PUT: api/SalesPipeline/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSalesPipeline(int id, SalesPipeline salesPipeline)
        {
            if (id != salesPipeline.PipelineId)
            {
                return BadRequest();
            }

            // Check if the Lead exists
            var lead = await _context.Leads.FirstOrDefaultAsync(l => l.LeadId == salesPipeline.LeadId);
            if (lead == null)
            {
                return BadRequest($"Lead with ID {salesPipeline.LeadId} does not exist.");
            }

            // Check if the Assigned User exists and is not deleted
            var userExists = await _context.Users.AnyAsync(u => u.UserId == lead.AssignedTo);
            if (!userExists)
            {
                return BadRequest($"User with ID {lead.AssignedTo} does not exist or is deleted.");
            }

            _context.Entry(salesPipeline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesPipelineExists(id))
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

        // DELETE: api/SalesPipeline/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesPipeline(int id)
        {
            var salesPipeline = await _context.SalesPipelines.FindAsync(id);
            if (salesPipeline == null)
            {
                return NotFound();
            }

            _context.SalesPipelines.Remove(salesPipeline);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalesPipelineExists(int id)
        {
            return _context.SalesPipelines.Any(e => e.PipelineId == id);
        }
    }
}
