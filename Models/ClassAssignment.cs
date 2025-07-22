using project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class ClassAssignment
    {
        [Key, Column(Order = 0)]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Key, Column(Order = 1)]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        // Optional: role or assignment date
        public DateTime? AssignedDate { get; set; }
    }
}
