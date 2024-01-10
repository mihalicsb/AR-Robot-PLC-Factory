using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs.plctype
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
        public PlcType PlcType { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.PlcTypes == null || PlcType == null)
            {
                return Page();
            }

            _context.PlcTypes.Add(PlcType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
