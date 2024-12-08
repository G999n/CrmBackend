using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class SalesPipeline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PipelineId { get; set; }

        [Required]
        public int LeadId { get; set; }  // Foreign key reference to the Lead table

        [Required]
        [MaxLength(50)]
        public required string Stage { get; set; }  // The current stage of the pipeline (e.g., Qualification, Proposal, Negotiation)

        [Required]
        public decimal EstimatedValue { get; set; }  // The estimated value of the sale or deal

        [Required]
        public DateTime ClosingDate { get; set; }  // The expected closing date of the deal

        [Required]
        [MaxLength(20)]
        public required string Status { get; set; }  // The status of the pipeline (e.g., Active, Closed, Won, Lost)
    }
}
