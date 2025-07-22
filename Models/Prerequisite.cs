using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Prerequisite
    {
        [Key, Column(Order = 0)]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Key, Column(Order = 1)]
        public int PrerequisiteCourseId { get; set; }
        public Course PrerequisiteCourse { get; set; }
    }
}
