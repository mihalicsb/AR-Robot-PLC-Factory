using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot.factory
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
        public Factory Factory { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Factories == null || Factory == null)
            {
                return Page();
            }

            _context.Factories.Add(Factory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
