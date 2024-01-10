using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs.ioports
{
    public class IndexModel : PageModel
    {
        private readonly DiplomaContext _context;

        public IndexModel(DiplomaContext context)
        {
            _context = context;
        }

        public IList<IoPort> IoPort { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.IoPorts != null)
            {
                IoPort = await _context.IoPorts
                .Include(i => i.Plc).ToListAsync();
            }
        }
    }
}
