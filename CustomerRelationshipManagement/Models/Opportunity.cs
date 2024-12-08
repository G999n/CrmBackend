using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class Opportunity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OpportunityId { get; set; }

        [Required]
        public int CustomerId { get; set; }  // Foreign key to the Customer table

        [Required]
        public int AccountManagerId { get; set; }  // Foreign key to the User table (Account Manager)

        [Required]
        public decimal OpportunityValue { get; set; }  // Value of the opportunity

        [Required]
        public DateTime CloseDate { get; set; }  // The expected close date of the opportunity

        [Required]
        [MaxLength(50)]
        public required string Stage { get; set; }  // The current stage of the opportunity (e.g., "Prospecting", "Negotiation")
    }
}
