using Project.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace project.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        // Navigation property to Faculty
        [Required]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        // Navigation property to ClassStudent join table
        public ICollection<ClassStudent> ClassStudents { get; set; }
    }
}
