using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ManagementWebServer.Models;
using QRCoder;
using System.Drawing;

namespace ManagementWebServer.Pages.plcs
{
    public class CreateModel : PageModel
    {
        private readonly DiplomaContext _context;
        private IWebHostEnvironment _env;
        public CreateModel(DiplomaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult OnGet()
        {
            ViewData["TypeId"] = new SelectList(_context.PlcTypes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Plc Plc { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Plc.Type = _context.PlcTypes.FirstOrDefault(p => p.Id == Plc.TypeId)!;
            //if (!ModelState.IsValid || _context.Plcs == null || Plc == null)
            //{
            //    return Page();
            //}

            _context.Plcs.Add(Plc);
            await _context.SaveChangesAsync();
            GenerateQR(Plc.Id);
            return RedirectToPage("./Index");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private void GenerateQR(int id)
        {
            //Path.Combine(_env.WebRootPath, @"qr");
            string wwwPath = _env.WebRootPath;
            string dir = "qr";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(id.ToString(), QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);

            Bitmap qrCodeImage;
            using (var ms = new MemoryStream(qrCodeAsBitmapByteArr))
            {
                qrCodeImage = new Bitmap(ms);

                qrCodeImage?.Save(@$"{wwwPath}\{dir}\{id}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}
