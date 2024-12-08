using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class Lead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int LeadId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LeadSource { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public required string ContactDetails { get; set; }

        [Required]
        [MaxLength(20)]
        public required string LeadStatus { get; set; }

        [Required]
        public int AssignedTo { get; set; } //foreign key to user_id 

        [Required]
        public required decimal PotentialValue { get; set; }

        [Required]
        public bool IsConverted { get; set; } //marks if lead is converted to customer
    }
}

