using System.ComponentModel.DataAnnotations;

namespace CIS325_Master_Web.Models
{
    /// <summary>Represents a course offered at Flagler College.</summary>
    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(12)]
        public string Code { get; set; } = string.Empty;       // e.g., CIS325

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;      // Full course title

        [Range(0, 6)]
        public int Credits { get; set; } = 3;                  // Credit hours

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>(); // Navigation property
    }
}
