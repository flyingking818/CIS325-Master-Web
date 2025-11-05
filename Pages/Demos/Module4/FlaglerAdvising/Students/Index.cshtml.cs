using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CIS325_Master_Web.Data;
using CIS325_Master_Web.Models;
using Microsoft.EntityFrameworkCore;



namespace CIS325_Master_Web.Pages.Demos.Module4.FlaglerAdvising.Students
{
    public class IndexModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }


        private readonly CIS325_Master_Web.Data.AdvisingContext _context;

        public IndexModel(CIS325_Master_Web.Data.AdvisingContext context)
        {
            _context = context;
        }
            
        public IList<Student> Student { get;set; } = default!;

        public async Task OnGetAsync()  // Called when the page is accessed via GET
        {
            // IQueryable allows building dynamic queries before execution
            // Make sure to include the object using <Student>
            IQueryable<Student> query = _context.Students.AsNoTracking();  // Retrieve data without tracking for better performance

            if (!string.IsNullOrWhiteSpace(SearchTerm))  // If a search term was entered
            {
                var term = SearchTerm.Trim();  // Remove extra spaces
                query = query.Where(s =>       // Filter results by first name, last name, or email (case-insensitive)
                    EF.Functions.Like(s.FirstName, $"%{term}%") ||
                    EF.Functions.Like(s.LastName, $"%{term}%") ||
                    EF.Functions.Like(s.Email, $"%{term}%"));
            }

            Student = await query              // Execute the query asynchronously
                .OrderBy(s => s.LastName)      // Sort by last name
                .ThenBy(s => s.FirstName)      // Then by first name
                .ToListAsync();                // Convert the results to a list
        }
    }
}
