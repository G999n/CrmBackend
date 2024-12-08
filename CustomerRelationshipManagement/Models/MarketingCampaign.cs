using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class MarketingCampaign
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CampaignId { get; set; }  // The primary key for the MarketingCampaigns table

        [Required]
        [MaxLength(100)]  // Assuming a maximum length for campaign name
        public required string Name { get; set; }  // Name of the campaign

        [Required]
        public DateTime StartDate { get; set; }  // Start date of the campaign

        [Required]
        public DateTime EndDate { get; set; }  // End date of the campaign

        [Required]
        [MaxLength(100)]  // Assuming a maximum length for target segment
        public required string TargetSegment { get; set; }  // The target segment of the campaign (e.g., "Young Adults", "Businesses")

        [Required]
        public decimal Budget { get; set; }  // The budget allocated for the campaign
    }
}
