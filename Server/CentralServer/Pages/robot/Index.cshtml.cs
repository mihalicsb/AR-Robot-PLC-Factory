using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot
{
    public class IndexModel : PageModel
    {
        private readonly DiplomaContext _context;

        public IndexModel(DiplomaContext context)
        {
            _context = context;
        }

        public IList<Robot> Robot { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Robots != null)
            {
                Robot = await _context.Robots
                .Include(r => r.Factory)
                .Include(r => r.Type).ToListAsync();
            }
        }
    }
}
