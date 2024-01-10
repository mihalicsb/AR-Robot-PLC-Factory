using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs
{
    public class EditModel : PageModel
    {
        private readonly DiplomaContext _context;
        private IWebHostEnvironment _env;
        public EditModel(DiplomaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Plc Plc { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Plcs == null)
            {
                return NotFound();
            }

            var plc = await _context.Plcs.FirstOrDefaultAsync(m => m.Id == id);
            if (plc == null)
            {
                return NotFound();
            }
            Plc = plc;
            ViewData["TypeId"] = new SelectList(_context.PlcTypes, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Plc.Type = _context.PlcTypes.FirstOrDefault(p => p.Id == Plc.TypeId)!;
            //if (!ModelState.IsValid)
            //{
            //    foreach (var error in ModelState.Values.SelectMany(modelState => modelState.Errors))
            //    {
            //        Console.WriteLine(error.ErrorMessage);
            //    }
            //    return Page();
            //}
           

            _context.Attach(Plc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlcExists(Plc.Id))
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

        private bool PlcExists(int id)
        {
            return (_context.Plcs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
