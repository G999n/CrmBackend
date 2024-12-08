using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public required string Company { get; set; }

        [MaxLength(50)]
        public required string Industry { get; set; }

        [Required]
        [MaxLength(200)]
        public required string ContactDetails { get; set; }

        [Required]
        [MaxLength(20)]
        public required string AccountStatus { get; set; }

        [Required]
        public required DateTime LastContactDate { get; set; }
    }
}
