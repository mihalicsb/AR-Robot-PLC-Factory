using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs.plctype
{
    public class EditModel : PageModel
    {
        private readonly DiplomaContext _context;

        public EditModel(DiplomaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PlcType PlcType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PlcTypes == null)
            {
                return NotFound();
            }

            var plctype =  await _context.PlcTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (plctype == null)
            {
                return NotFound();
            }
            PlcType = plctype;
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

            _context.Attach(PlcType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlcTypeExists(PlcType.Id))
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

        private bool PlcTypeExists(int id)
        {
          return (_context.PlcTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
