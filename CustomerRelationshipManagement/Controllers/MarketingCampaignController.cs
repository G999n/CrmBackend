using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerRelationshipManagement.Models;
using System.Threading.Tasks;
using CustomerRelationshipManagement.Data;
using System.Linq;

namespace CustomerRelationshipManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketingCampaignController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public MarketingCampaignController(CrmDbContext context)
        {
            _context = context;
        }

        // POST: api/MarketingCampaign
        [HttpPost]
        public async Task<IActionResult> CreateMarketingCampaign([FromBody] MarketingCampaign campaign)
        {
            // Validate campaign dates
            if (campaign.StartDate >= campaign.EndDate)
            {
                return BadRequest(new { message = "Start date must be earlier than the end date." });
            }

            // Add campaign to the database
            _context.MarketingCampaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMarketingCampaignById), new { id = campaign.CampaignId }, campaign);
        }

        // GET: api/MarketingCampaign/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MarketingCampaign>> GetMarketingCampaignById(int id)
        {
            var campaign = await _context.MarketingCampaigns.FindAsync(id);

            if (campaign == null)
            {
                return NotFound(new { message = "Marketing campaign not found." });
            }

            return Ok(campaign);
        }

        // GET: api/MarketingCampaign
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarketingCampaign>>> GetAllMarketingCampaigns()
        {
            var campaigns = await _context.MarketingCampaigns.ToListAsync();

            // Return an empty list if no campaigns are found
            return Ok(campaigns);  // campaigns will be an empty list if no campaigns are found
        }

        // PUT: api/MarketingCampaign/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMarketingCampaign(int id, [FromBody] MarketingCampaign campaign)
        {
            if (id != campaign.CampaignId)
            {
                return BadRequest(new { message = "Campaign ID mismatch." });
            }

            // Validate campaign dates
            if (campaign.StartDate >= campaign.EndDate)
            {
                return BadRequest(new { message = "Start date must be earlier than the end date." });
            }

            _context.Entry(campaign).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarketingCampaignExists(id))
                {
                    return NotFound(new { message = "Marketing campaign not found." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(campaign); // Successfully updated
        }

        // DELETE: api/MarketingCampaign/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarketingCampaign(int id)
        {
            var campaign = await _context.MarketingCampaigns.FindAsync(id);

            if (campaign == null)
            {
                return NotFound(new { message = "Marketing campaign not found." });
            }

            _context.MarketingCampaigns.Remove(campaign);
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully deleted
        }

        // Helper method to check if a marketing campaign exists
        private bool MarketingCampaignExists(int id)
        {
            return _context.MarketingCampaigns.Any(c => c.CampaignId == id);
        }
    }
}
