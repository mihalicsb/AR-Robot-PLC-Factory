using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot.robottpye
{
    public class EditModel : PageModel
    {
        private readonly DiplomaContext _context;

        public EditModel(DiplomaContext context)
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

            var robottype =  await _context.RobotTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (robottype == null)
            {
                return NotFound();
            }
            RobotType = robottype;
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

            _context.Attach(RobotType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RobotTypeExists(RobotType.Id))
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

        private bool RobotTypeExists(int id)
        {
          return (_context.RobotTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
