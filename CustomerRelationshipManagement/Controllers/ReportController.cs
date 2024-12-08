using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerRelationshipManagement.Models;
using System.Threading.Tasks;
using CustomerRelationshipManagement.Data;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public ReportController(CrmDbContext context)
        {
            _context = context;
        }

        // POST: api/Report
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] Report report)
        {
            if (report.GeneratedDate > DateTime.Now)
            {
                return BadRequest(new { message = "Generated date cannot be in the future." });
            }

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportById), new { id = report.ReportId }, report);
        }

        // GET: api/Report/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReportById(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound(new { message = "Report not found." });
            }

            return Ok(report);
        }

        // PUT: api/Report/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] Report report)
        {
            if (id != report.ReportId)
            {
                return BadRequest(new { message = "Report ID mismatch." });
            }

            if (report.GeneratedDate > DateTime.Now)
            {
                return BadRequest(new { message = "Generated date cannot be in the future." });
            }

            _context.Entry(report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
                {
                    return NotFound(new { message = "Report not found." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Report/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound(new { message = "Report not found." });
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to check if a report exists
        private bool ReportExists(int id)
        {
            return _context.Reports.Any(r => r.ReportId == id);
        }
    }
}
