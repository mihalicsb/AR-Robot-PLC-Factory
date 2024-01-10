using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot.factory
{
    public class IndexModel : PageModel
    {
        private readonly DiplomaContext _context;

        public IndexModel(DiplomaContext context)
        {
            _context = context;
        }

        public IList<Factory> Factory { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Factories != null)
            {
                Factory = await _context.Factories.ToListAsync();
            }
        }
    }
}
