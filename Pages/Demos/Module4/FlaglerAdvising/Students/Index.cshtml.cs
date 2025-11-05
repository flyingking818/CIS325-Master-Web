using CIS325_Master_Web.Data;
using CIS325_Master_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;



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

        //Option 1- Use EF.Functions.Like() (SQL-level, case-insensitive by default)
        //Pretty efficient (runs entirely in SQL),
        //automatically case-insensitive for typical collations
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

        //Option 2 - Use the Contains() method (it's case-sensitive)
        // Still runs in SQL
        // Guaranteed case-insensitive regardless of collation
        // Slightly less efficient because of ToLower() on each field

        /*
        public async Task OnGetAsync()
        {
            IQueryable<Student> query = _context.Students.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var term = SearchTerm.Trim().ToLower();

                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(term) ||
                    s.LastName.ToLower().Contains(term) ||
                    s.Email.ToLower().Contains(term));
            }

            Student = await query
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();
        }
        */
        //Option 3 – Use Contains(StringComparison.OrdinalIgnoreCase) (in-memory)
        //Good only for small datasets or when you need full.NET string comparison features
        //Similar to the earlier one we did for the list search. :)

        /*
        public async Task OnGetAsync()
        {
            var allStudents = await _context.Students.AsNoTracking().ToListAsync();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var term = SearchTerm.Trim();

                Student = allStudents
                    .Where(s =>
                        s.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        s.LastName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        s.Email.Contains(term, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(s => s.LastName)
                    .ThenBy(s => s.FirstName)
                    .ToList();
            }
            else
            {
                Student = allStudents
                    .OrderBy(s => s.LastName)
                    .ThenBy(s => s.FirstName)
                    .ToList();
            }
        }
        */

    }
}
