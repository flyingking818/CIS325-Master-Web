using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CIS325_Master_Web.Data;
using CIS325_Master_Web.Models;

namespace CIS325_Master_Web.Pages.Demos.Module4.FlaglerAdvising.Students
{
    public class CreateModel : PageModel
    {
        private readonly CIS325_Master_Web.Data.AdvisingContext _context;

        public CreateModel(CIS325_Master_Web.Data.AdvisingContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Students.Add(Student);
            await _context.SaveChangesAsync(); //This commits!

            return RedirectToPage("./Index");
        }
    }
}
