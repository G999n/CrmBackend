using CustomerRelationshipManagement.Data;
using CustomerRelationshipManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationHistoryController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public CommunicationHistoryController(CrmDbContext context)
        {
            _context = context;
        }

        // GET: api/CommunicationHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunicationHistory>>> GetCommunicationHistories()
        {
            return await _context.CommunicationHistories.ToListAsync();
        }

        // GET: api/CommunicationHistory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommunicationHistory>> GetCommunicationHistory(int id)
        {
            var communicationHistory = await _context.CommunicationHistories.FindAsync(id);

            if (communicationHistory == null)
            {
                return NotFound();
            }

            return communicationHistory;
        }

        // POST: api/CommunicationHistory
        [HttpPost]
        public async Task<ActionResult<CommunicationHistory>> CreateCommunicationHistory(CommunicationHistory communicationHistory)
        {
            // Validate that CustomerId exists in the Customers table
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == communicationHistory.CustomerId);
            if (!customerExists)
            {
                return BadRequest($"Customer with ID {communicationHistory.CustomerId} does not exist.");
            }

            _context.CommunicationHistories.Add(communicationHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommunicationHistory), new { id = communicationHistory.InteractionId }, communicationHistory);
        }

        // PUT: api/CommunicationHistory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommunicationHistory(int id, CommunicationHistory communicationHistory)
        {
            if (id != communicationHistory.InteractionId)
            {
                return BadRequest();
            }

            // Validate that CustomerId exists in the Customers table
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == communicationHistory.CustomerId);
            if (!customerExists)
            {
                return BadRequest($"Customer with ID {communicationHistory.CustomerId} does not exist.");
            }

            _context.Entry(communicationHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunicationHistoryExists(id))
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

        // DELETE: api/CommunicationHistory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunicationHistory(int id)
        {
            var communicationHistory = await _context.CommunicationHistories.FindAsync(id);
            if (communicationHistory == null)
            {
                return NotFound();
            }

            _context.CommunicationHistories.Remove(communicationHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommunicationHistoryExists(int id)
        {
            return _context.CommunicationHistories.Any(e => e.InteractionId == id);
        }
    }
}
