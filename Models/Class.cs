using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [MaxLength(10)]
        public string RoomNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Building { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [MaxLength(20)]
        public string Term { get; set; }

        [Required]
        public int TimeSlotId { get; set; }
        public TimeSlot TimeSlot { get; set; }

        // Navigation to student enrollments
        public ICollection<ClassStudent> ClassStudents { get; set; }

        // Navigation to teacher assignments
        public ICollection<ClassAssignment> ClassAssignments { get; set; }
    }
}
