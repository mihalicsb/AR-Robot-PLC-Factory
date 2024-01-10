using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ManagementWebServer.Models;


namespace ManagementWebServer.Pages.robot
{
    public class CreateModel : PageModel
    {
        private readonly DiplomaContext _context;
       

        public CreateModel(DiplomaContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["FactoryId"] = new SelectList(_context.Factories, "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.RobotTypes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Robot Robot { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Robot.Type = _context.RobotTypes.FirstOrDefault(rt => rt.Id == Robot.TypeId)!;
            Robot.Factory = _context.Factories.FirstOrDefault(f => f.Id == Robot.FactoryId)!;
            //if (!ModelState.IsValid || _context.Robots == null || Robot == null)
            if (_context.Robots == null || Robot == null)
            {
                return Page();
            }

            _context.Robots.Add(Robot);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
