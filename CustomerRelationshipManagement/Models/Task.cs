using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required]
        public int CustomerId { get; set; } // Foreign key to the Customer table

        [Required]
        public int AssignedTo { get; set; } // Foreign key to the User table

        [Required]
        [MaxLength(500)]
        public required string TaskDescription { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Status { get; set; }

        [Required]
        public int Priority { get; set; } //integer - higher means higher priority
    }
}
