using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CIS325_Master_Web.Pages.Demos.Module2
{
    public class GreetingModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage ="Name is required!")]
        public string Name { get; set; } = string.Empty;
        public string ConfirmationMessage{ get; set; }

        public void OnGet()
        {
        }

        public void OnPost() {
            //Are there any validation errors?
            if (!ModelState.IsValid)
            {
                return;
            }

            ConfirmationMessage = $"Hello, {Name}, welcome to Flagler College";
            

        }
    }
}
