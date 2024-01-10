using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs.ioports
{
    public class EditModel : PageModel
    {
        private readonly DiplomaContext _context;

        public EditModel(DiplomaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IoPort IoPort { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.IoPorts == null)
            {
                return NotFound();
            }

            var ioport =  await _context.IoPorts.FirstOrDefaultAsync(m => m.Id == id);
            if (ioport == null)
            {
                return NotFound();
            }
            IoPort = ioport;
           ViewData["PlcId"] = new SelectList(_context.Plcs, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IoPort.Plc = _context.Plcs.FirstOrDefault(p => p.Id == IoPort.PlcId)!;
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            _context.Attach(IoPort).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IoPortExists(IoPort.Id))
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

        private bool IoPortExists(int id)
        {
          return (_context.IoPorts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
