using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs
{
    public class DeleteModel : PageModel
    {
        private readonly DiplomaContext _context;

        public DeleteModel(DiplomaContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Plc Plc { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Plcs == null)
            {
                return NotFound();
            }

            var plc = await _context.Plcs.FirstOrDefaultAsync(m => m.Id == id);

            if (plc == null)
            {
                return NotFound();
            }
            else 
            {
                Plc = plc;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Plcs == null)
            {
                return NotFound();
            }
            var plc = await _context.Plcs.FindAsync(id);

            if (plc != null)
            {
                Plc = plc;
                _context.Plcs.Remove(Plc);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
