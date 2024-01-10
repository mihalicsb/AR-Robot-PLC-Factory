using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot
{
    public class EditModel : PageModel
    {
        private readonly DiplomaContext _context;
        

        public EditModel(DiplomaContext context)
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

            var robot =  await _context.Robots.FirstOrDefaultAsync(m => m.Guid == id);
            if (robot == null)
            {
                return NotFound();
            }
            Robot = robot;
            ViewData["FactoryId"] = new SelectList(_context.Factories, "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.RobotTypes, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Robot.Type = _context.RobotTypes.FirstOrDefault(rt => rt.Id == Robot.TypeId)!;
            Robot.Factory = _context.Factories.FirstOrDefault(f => f.Id == Robot.FactoryId)!;
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            _context.Attach(Robot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RobotExists(Robot.Guid))
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


        private bool RobotExists(string id)
        {
          return (_context.Robots?.Any(e => e.Guid == id)).GetValueOrDefault();
        }

    }
}
