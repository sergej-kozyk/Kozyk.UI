using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kozyk.UI.Data;
using Srv.Domain.Entities;

namespace Kozyk.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly Kozyk.UI.Data.ApplicationDbContext _context;

        public EditModel(Kozyk.UI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Serving Serving { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serving =  await _context.Serving.FirstOrDefaultAsync(m => m.Id == id);
            if (serving == null)
            {
                return NotFound();
            }
            Serving = serving;
           ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "GroupName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Serving).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServingExists(Serving.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ServingExists(int id)
        {
            return _context.Serving.Any(e => e.Id == id);
        }
    }
}
