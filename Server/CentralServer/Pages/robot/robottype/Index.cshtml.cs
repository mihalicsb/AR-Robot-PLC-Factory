using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.robot.robottpye
{
    public class IndexModel : PageModel
    {
        private readonly DiplomaContext _context;

        public IndexModel(DiplomaContext context)
        {
            _context = context;
        }

        public IList<RobotType> RobotType { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.RobotTypes != null)
            {
                RobotType = await _context.RobotTypes.ToListAsync();
            }
        }
    }
}
