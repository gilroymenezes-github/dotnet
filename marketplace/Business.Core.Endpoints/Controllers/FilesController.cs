using Business.Shared.Storage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Business.Core.Endpoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        IBlobStorage blobStorage;
        private long maxFileSize = 8192 * 1000;

        public FilesController(IBlobStorage blobStorage)
        {
            this.blobStorage= blobStorage;
        }

        [HttpPost]
        [Route("/api/[controller]/upload-pdf/{id}")]
        public async Task<IActionResult> UploadPdfFile(IFormFile file)
        {
            if (!Request.HasFormContentType) return new UnsupportedMediaTypeResult();

            string id = (string)RouteData.Values["id"];

            var filename = $"{id}.pdf";

            using var ms = new MemoryStream();
            
            file.CopyTo(ms);
            
            ms.Seek(0, SeekOrigin.Begin);
            
            var blobname = await blobStorage.UploadBlobAsync(filename, ms);

            return Ok(blobname);
        }
    }
}
