using Microsoft.AspNetCore.Mvc;

namespace ManagementWebServer.Pages
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileApi : ControllerBase
    {
        private IWebHostEnvironment _env;
        private string wwwStoragePath;
        public FileApi(IWebHostEnvironment env)
        {
            _env = env;
            wwwStoragePath = Path.Combine(_env.WebRootPath, @"qr");

        }
        public FileResult OnGetDownloadQR(string fileName)
        {
            //TODO hibakezelés, ha olyan fájlt kér ami nem létezik
            byte[] bytes = System.IO.File.ReadAllBytes($"{wwwStoragePath}\\{fileName}");
            Console.WriteLine($"{DateTime.Now}: letöltés");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
          }
    }
}
