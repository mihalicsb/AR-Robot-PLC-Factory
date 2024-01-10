using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManagementWebServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using QRCoder;

namespace ManagementWebServer.Pages.plcs
{
    public class IndexModel : PageModel
    {
        private readonly DiplomaContext _context;

        private IWebHostEnvironment _env;
        public IndexModel(DiplomaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IList<Plc> Plc { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Plcs != null)
            {
                Plc = await _context.Plcs
                .Include(p => p.Type).ToListAsync();
            }
        }

        public FileResult OnGetDownloadFile(int id)
        {
            var filePath = Path.Combine(_env.WebRootPath, "qr", id + ".jpg");
            if(!System.IO.File.Exists(filePath))
            {
                GenerateQR(id);
            }
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", id + ".jpg");
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
