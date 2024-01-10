using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;

namespace ManagementWebServer.Pages.plcs.plctype
{
    public class IndexModel : PageModel
    {
        private readonly DiplomaContext _context;

        public IndexModel(DiplomaContext context)
        {
            _context = context;
        }

        public IList<PlcType> PlcType { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.PlcTypes != null)
            {
                PlcType = await _context.PlcTypes.ToListAsync();
            }
        }
    }
}
