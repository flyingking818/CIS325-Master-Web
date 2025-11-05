using System.ComponentModel.DataAnnotations;

namespace CIS325_Master_Web.Models
{
    // <summary>Represents a student entity stored in the database.</summary>
    public class Student
    {
        public int Id { get; set; }                           // Primary key

        [Required, StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty; // Student first name

        [Required, StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;  // Student last name

        [Required, EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;     // Valid email required

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";  // Computed read-only property

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>(); // 1-to-many relation
    }
}
