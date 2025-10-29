using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CIS325_Master_Web.Data;
using CIS325_Master_Web.Models;

namespace CIS325_Master_Web.Pages.Demos.Module4.FlaglerAdvising.Schedules
{
    public class DetailsModel : PageModel
    {
        private readonly CIS325_Master_Web.Data.AdvisingContext _context;

        public DetailsModel(CIS325_Master_Web.Data.AdvisingContext context)
        {
            _context = context;
        }

        public Schedule Schedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FirstOrDefaultAsync(m => m.Id == id);

            if (schedule is not null)
            {
                Schedule = schedule;

                return Page();
            }

            return NotFound();
        }
    }
}
