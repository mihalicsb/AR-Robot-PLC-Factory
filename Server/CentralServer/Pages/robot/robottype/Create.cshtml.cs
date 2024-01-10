using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot.robottpye
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
            return Page();
        }

        [BindProperty]
        public RobotType RobotType { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.RobotTypes == null || RobotType == null)
            {
                return Page();
            }

            _context.RobotTypes.Add(RobotType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
