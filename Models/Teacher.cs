using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

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
        public DateTime HireDate { get; set; }

        [Required]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        // حقوق استاد
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        // Navigation to classes assigned
        public ICollection<ClassAssignment> ClassAssignments { get; set; }
    }
}
