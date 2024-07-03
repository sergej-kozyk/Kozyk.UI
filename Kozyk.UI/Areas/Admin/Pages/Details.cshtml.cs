using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Kozyk.UI.Data;
using Srv.Domain.Entities;

namespace Kozyk.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly Kozyk.UI.Data.ApplicationDbContext _context;

        public DetailsModel(Kozyk.UI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Serving Serving { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serving = await _context.Serving.FirstOrDefaultAsync(m => m.Id == id);
            if (serving == null)
            {
                return NotFound();
            }
            else
            {
                Serving = serving;
            }
            return Page();
        }
    }
}
