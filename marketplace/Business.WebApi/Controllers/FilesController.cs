using Business.Shared.Connections;
using Business.Shared.Models;
using Business.Shared.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Business.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        IBlobStorage blobStorage;
        FilesTableStore filesStorage;
        FilesQueueClient filesCommand;
        
        public FilesController(IBlobStorage blobStorage, FilesTableStore filesStorage, FilesQueueClient filesCommand)
        {
            this.blobStorage= blobStorage;
            this.filesStorage = filesStorage;
            this.filesCommand = filesCommand;
        }

        [HttpPost]
        [Route("api/[controller]/upload/pdf")]
        public async Task<IActionResult> UploadPdfFile(IFormFile file)
        {
            if (!Request.HasFormContentType) return new UnsupportedMediaTypeResult();

            using var ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var blobUri = await blobStorage.UploadBlobAsync(file.FileName, file.ContentType, ms);

            return Ok(blobUri);
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> Post([FromBody] FileModel item)
        {
            await filesCommand.CreateAsync(item);
            var response = await filesStorage.CreateItemAsync(item);
            return Ok(response);
        }
    }
}
