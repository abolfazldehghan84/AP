using project.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class GradeAppeal
    {
        [Key]
        public int AppealId { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        [MaxLength(1000)]
        public string AppealText { get; set; }

        [MaxLength(1000)]
        public string InstructorResponse { get; set; }

        [Required]
        public DateTime SubmittedAt { get; set; }

        public DateTime? RespondedAt { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // e.g., "Pending", "Reviewed", "Rejected"
    }
}
