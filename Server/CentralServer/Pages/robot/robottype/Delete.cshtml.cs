using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot.robottpye
{
    public class DeleteModel : PageModel
    {
        private readonly DiplomaContext _context;

        public DeleteModel(DiplomaContext context)
        {
            _context = context;
        }

        [BindProperty]
      public RobotType RobotType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.RobotTypes == null)
            {
                return NotFound();
            }

            var robottype = await _context.RobotTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (robottype == null)
            {
                return NotFound();
            }
            else 
            {
                RobotType = robottype;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.RobotTypes == null)
            {
                return NotFound();
            }
            var robottype = await _context.RobotTypes.FindAsync(id);

            if (robottype != null)
            {
                RobotType = robottype;
                _context.RobotTypes.Remove(RobotType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
