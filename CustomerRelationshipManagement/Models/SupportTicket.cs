using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class SupportTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        [Required]
        public int CustomerId { get; set; }  // Foreign key to the Customer table

        [Required]
        [MaxLength(500)]  // Adjusted length for issue description
        public required string IssueDescription { get; set; }  // Description of the issue reported

        [Required]
        public int AssignedTo { get; set; }  // Foreign key to the User table (Assigned Support Agent)

        [Required]
        [MaxLength(50)]
        public required string TicketStatus { get; set; }  // Status of the ticket (e.g., Open, Closed, In Progress)

        public DateTime? ResolutionDate { get; set; }  // The date when the ticket was resolved (nullable)
    }
}
