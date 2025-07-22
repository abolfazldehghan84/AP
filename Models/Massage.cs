using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; }

        // Optional: message type (e.g., "manual", "system")
        [MaxLength(20)]
        public string Type { get; set; }
    }
}
