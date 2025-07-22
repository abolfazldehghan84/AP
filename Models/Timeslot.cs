using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class TimeSlot
    {
        [Key]
        public int TimeSlotId { get; set; }

        [Required]
        [MaxLength(10)]
        public string DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        // Navigation to classes scheduled in this time slot
        public ICollection<Class> Classes { get; set; }
    }
}
