using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class Report
    {
        [Key]  // Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        [Required]  // Ensures the field cannot be null
        [MaxLength(100)]  // Maximum length for the 'Type' field
        public required string Type { get; set; }

        [Required]
        public DateTime GeneratedDate { get; set; }

        [Required]
        public required string Data { get; set; }

        [Required]
        [MaxLength(50)]  // Maximum length for 'CreatedBy'
        public required string CreatedBy { get; set; }
    }
}
