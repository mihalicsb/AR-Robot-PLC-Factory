using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot.factory
{
    public class DeleteModel : PageModel
    {
        private readonly DiplomaContext _context;

        public DeleteModel(DiplomaContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Factory Factory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Factories == null)
            {
                return NotFound();
            }

            var factory = await _context.Factories.FirstOrDefaultAsync(m => m.Id == id);

            if (factory == null)
            {
                return NotFound();
            }
            else 
            {
                Factory = factory;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Factories == null)
            {
                return NotFound();
            }
            var factory = await _context.Factories.FindAsync(id);

            if (factory != null)
            {
                Factory = factory;
                _context.Factories.Remove(Factory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
