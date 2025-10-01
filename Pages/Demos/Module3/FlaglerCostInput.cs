/*
FlaglerCostInput:
    This is our class used to capture user input from the form (Tuition, Room, MealPlan, etc.).
    It uses DataAnnotations ([Required], [Range], etc.) to validate inputs.

FlaglerCostResult:
    This is our calculation result class (output).
    It holds numeric properties like Tuition, Room, MealPlan, Others, Scholarship, and Total.
 */

//This directive allows us to add [Required] etc. for validations.
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CIS325_Master_Web.Pages.Demos.Module3
{
    //Add the choice list
    public enum EmailChoice { No = 0, Yes = 1 }

    //Add a reusable custom attridute for RequiredIf (don't worry about the
    //inner workings. Let's just use it like a new library class!)
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _dependentProperty;
        private readonly object? _targetValue;

        public RequiredIfAttribute(string dependentProperty, object? targetValue)
        {
            _dependentProperty = dependentProperty;
            _targetValue = targetValue;
            ErrorMessage = "{0} is required.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var depProp = instance.GetType().GetProperty(_dependentProperty, BindingFlags.Public | BindingFlags.Instance);
            if (depProp == null) return ValidationResult.Success; // misconfigured: fail silently

            var dependentValue = depProp.GetValue(instance, null);
            var shouldRequire = Equals(dependentValue, _targetValue);

            if (!shouldRequire) return ValidationResult.Success;

            var hasValue =
                value is string s ? !string.IsNullOrWhiteSpace(s) :
                value is IEnumerable<object> e ? e.Any() :
                value != null;

            return hasValue ? ValidationResult.Success
                            : new ValidationResult(string.Format(ErrorMessageString, context.DisplayName));
        }
    }


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

        // ===============Additional fields for email notifications using custom attribute==========================
        [Required(ErrorMessage = "Please choose whether to receive results by email.")]
        public EmailChoice EmailResult { get; set; } = EmailChoice.No;

        [Display(Name = "Your Name")]
        [RequiredIf(nameof(EmailResult), EmailChoice.Yes, ErrorMessage = "Name is required when emailing results.")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RequiredIf(nameof(EmailResult), EmailChoice.Yes, ErrorMessage = "Email is required when emailing results.")]
        public string? Email { get; set; }
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
