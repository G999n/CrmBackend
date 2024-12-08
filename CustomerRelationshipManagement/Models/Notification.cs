using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagement.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }  // Primary Key

        [Required]
        [MaxLength(50)]
        public required string Type { get; set; }  // Type of notification (e.g., Lead Conversion, Task Deadline)

        [Required]
        [MaxLength(500)]
        public required string Message { get; set; }  // Notification message content

        [Required]
        public DateTime Timestamp { get; set; }  // Timestamp of the notification

        [Required]
        public int UserId { get; set; }  // Foreign key to User table (recipient of the notification)

        [Required]
        public bool IsRead { get; set; }  // To track if the notification has been read
    }
}
