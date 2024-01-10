using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs.plctype
{
    public class DeleteModel : PageModel
    {
        private readonly DiplomaContext _context;

        public DeleteModel(DiplomaContext context)
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

            var plctype = await _context.PlcTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (plctype == null)
            {
                return NotFound();
            }
            else 
            {
                PlcType = plctype;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.PlcTypes == null)
            {
                return NotFound();
            }
            var plctype = await _context.PlcTypes.FindAsync(id);

            if (plctype != null)
            {
                PlcType = plctype;
                _context.PlcTypes.Remove(PlcType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
