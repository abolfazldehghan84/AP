using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class ClassStudent
    {
        [Key, Column(Order = 0)]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Key, Column(Order = 1)]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // Optional grade field
        public double? Grade { get; set; }
    }
}
