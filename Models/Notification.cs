using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public int UserId { get; set; } // Can refer to either a student or a teacher

        [Required]
        [MaxLength(500)]
        public string Message { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; }

        [MaxLength(20)]
        public string Type { get; set; } // e.g., "system", "alert", "info"
    }
}
