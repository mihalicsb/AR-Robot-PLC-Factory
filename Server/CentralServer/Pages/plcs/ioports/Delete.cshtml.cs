using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs.ioports
{
    public class DeleteModel : PageModel
    {
        private readonly DiplomaContext _context;

        public DeleteModel(DiplomaContext context)
        {
            _context = context;
        }

        [BindProperty]
      public IoPort IoPort { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.IoPorts == null)
            {
                return NotFound();
            }

            var ioport = await _context.IoPorts.FirstOrDefaultAsync(m => m.Id == id);

            if (ioport == null)
            {
                return NotFound();
            }
            else 
            {
                IoPort = ioport;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.IoPorts == null)
            {
                return NotFound();
            }
            var ioport = await _context.IoPorts.FindAsync(id);

            if (ioport != null)
            {
                IoPort = ioport;
                _context.IoPorts.Remove(IoPort);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
