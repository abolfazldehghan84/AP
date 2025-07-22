
using project.Models;
using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace Project.Models
    {
        public class Course
        {
            [Key]
            public int CourseId { get; set; }

            [Required]
            [MaxLength(10)]
            public string CourseCode { get; set; }

            [Required]
            [MaxLength(100)]
            public string Title { get; set; }

            [Required]
            public int Credits { get; set; }

            [MaxLength(1000)]
            public string Description { get; set; }

            // Navigation property to Faculty (Department)
            [Required]
            public int FacultyId { get; set; }
            public Faculty Faculty { get; set; }

            // Navigation to classes offered for this course
            public ICollection<Class> Classes { get; set; }

            // Navigation for prerequisites (many-to-many self-referencing)
            public ICollection<Prerequisite> Prerequisites { get; set; }
            public ICollection<Prerequisite> IsPrerequisiteFor { get; set; }
        }
    }


