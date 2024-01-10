using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot
{
    public class DeleteModel : PageModel
    {
        private readonly DiplomaContext _context;

        public DeleteModel(DiplomaContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Robot Robot { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Robots == null)
            {
                return NotFound();
            }

            var robot = await _context.Robots.FirstOrDefaultAsync(m => m.Guid == id);

            if (robot == null)
            {
                return NotFound();
            }
            else 
            {
                Robot = robot;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Robots == null)
            {
                return NotFound();
            }
            var robot = await _context.Robots.FindAsync(id);

            if (robot != null)
            {
                Robot = robot;
                _context.Robots.Remove(Robot);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
