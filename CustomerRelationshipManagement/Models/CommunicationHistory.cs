using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class CommunicationHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InteractionId { get; set; }  // Primary key for the communication record

        [Required]
        public int CustomerId { get; set; }  // Foreign key reference to the Customer table

        [Required]
        [MaxLength(50)]
        public required string InteractionType { get; set; }  // Type of interaction (e.g., Email, Call, Meeting)

        [Required]
        public DateTime Date { get; set; }  // Date of the interaction

        [Required]
        [MaxLength(500)]
        public required string Notes { get; set; }  // Notes or details of the interaction

        [Required]
        public bool FollowUpRequired { get; set; }  // Whether follow-up is required after this interaction
    }
}
