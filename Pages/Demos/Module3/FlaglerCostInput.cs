using System.ComponentModel.DataAnnotations;

namespace CIS325_Master_Web.Pages.Demos.Module3
{
    // ===============================
    // Model class for capturing user inputs
    // ===============================
    public class FlaglerCostInput
    {
        // Tuition is required, must be between 0 and 50,000.
        // The property is nullable (double?) so the field can start blank in the form
        // and only gets a value once the user provides input.
        // The "!" operator will be used later to suppress null warnings
        // after validation ensures the value is present.
        [Required, Range(0, 50000, ErrorMessage = "Tuition must be between 0 and $50000.")]
        public double? Tuition { get; set; }     // nullable => renders blank

        // The student must select a Room option.
        // Initialized with "" to avoid null reference issues on first render.
        [Required]
        public string Room { get; set; } = "";

        // The student must select a Meal Plan option.
        // Initialized with "" for the same reason as Room.
        [Required]
        public string MealPlan { get; set; } = "";

        // List of optional other expenses (Fees, Books, Transportation, etc.)
        // Students can select multiple options; defaults to an empty list.
        public List<string> OtherExpenses { get; set; } = new();

        // Scholarship amount is required and must be between 0 and 100,000.
        // Nullable type allows the field to render blank before input.
        [Required, Range(0, 100000, ErrorMessage = "Scholarship must be between 0 and 100,000.")]
        public double? Scholarship { get; set; }   // nullable => renders blank
    }

    // ===============================
    // Model class for calculation results
    // ===============================
    public class FlaglerCostResult
    {
        // These properties confirm the user’s inputs after form submission
        public double Tuition { get; set; }
        public double Room { get; set; }
        public double MealPlan { get; set; }
        public double Others { get; set; }
        public double Scholarship { get; set; }

        // This property holds the final calculated total:
        // Tuition + Room + Meal + Others – Scholarship
        public double Total { get; set; }
    }
}
