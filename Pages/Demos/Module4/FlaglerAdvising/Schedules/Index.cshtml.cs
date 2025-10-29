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
    public class IndexModel : PageModel
    {
        private readonly CIS325_Master_Web.Data.AdvisingContext _context;

        public IndexModel(CIS325_Master_Web.Data.AdvisingContext context)
        {
            _context = context;
        }

        public IList<Schedule> Schedule { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Schedule = await _context.Schedules
                .Include(s => s.Course)
                .Include(s => s.Student).ToListAsync();
        }
    }
}
