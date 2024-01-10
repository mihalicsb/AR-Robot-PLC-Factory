using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ManagementWebServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementWebServer.Pages.plcs.ioports
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
        ViewData["PlcId"] = new SelectList(_context.Plcs, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public IoPort IoPort { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IoPort.Plc = _context.Plcs.Include(p => p.Type).FirstOrDefault(p => p.Id == IoPort.PlcId)!;
            //if (!ModelState.IsValid || _context.IoPorts == null || IoPort == null)
            if (_context.IoPorts == null || IoPort == null)
            {
                return Page();
            }

            _context.IoPorts.Add(IoPort);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
