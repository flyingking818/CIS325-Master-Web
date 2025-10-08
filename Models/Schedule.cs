using System.ComponentModel.DataAnnotations;

namespace CIS325_Master_Web.Models
{
    /// <summary>Represents a student’s course schedule entry for a term.</summary>
    public class Schedule
    {
        public int Id { get; set; }

        [Display(Name = "Student")]
        public int StudentId { get; set; }                    // FK to Student

        [Display(Name = "Course")]
        public int CourseId { get; set; }                     // FK to Course

        [Required, StringLength(12)]
        public string Term { get; set; } = "2026SP";          // Term code

        [Required, StringLength(10)]
        public string DayOfWeek { get; set; } = "Mon";        // Class day

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; } = new(9, 0, 0); // Start time

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; } = new(9, 50, 0); // End time

        public Student? Student { get; set; }                 // Navigation to Student
        public Course? Course { get; set; }                   // Navigation to Course
    }
}
