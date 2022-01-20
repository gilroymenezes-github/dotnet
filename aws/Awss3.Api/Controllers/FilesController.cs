using Awss3.Api.Contracts.Files;
using Awss3.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Awss3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository filesRepository;

        public FilesController(IFilesRepository filesRepository)
        {
            this.filesRepository = filesRepository;
        }

        [HttpPost]
        [Route("{bucketName}/add")]
        public async Task<ActionResult<AddFileListResponse>> AddFiles(string bucketName, IList<IFormFile> formFiles)
        {
            if (formFiles is null) return BadRequest("The request has no files to be uploaded");

            var response = await filesRepository.UploadFiles(bucketName, formFiles);

            if (response is null) return BadRequest();

            return Ok(response);
        }

        [HttpGet]
        [Route("{bucketName}/list")]
        public async Task<ActionResult<IEnumerable<GetFileListResponse>>> ListFiles(string bucketName)
        {
            var response = await filesRepository.ListFiles(bucketName);

            return Ok(response);
        }

        [HttpGet]
        [Route("{bucketName}/download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string bucketName, string fileName)
        {
            await filesRepository.DownloadFile(bucketName, fileName);

            return Ok();
        }

        [HttpDelete]
        [Route("{bucketName}/delete/{fileName}")]
        public async Task<ActionResult<DeleteFileResponse>> DeleteFile(string bucketName, string fileName)
        {
            var response = await filesRepository.DeleteFile(bucketName, fileName);

            return Ok(response);
        }

        [HttpPost]
        [Route("{bucketName}/addJsonObject")]
        public async Task<IActionResult> AddJsonObject(string bucketName, AddJsonObjectRequest request)
        {
            await filesRepository.AddJsonObject(bucketName, request);

            return Ok();
        }

        [HttpGet]
        [Route("{bucketName}/getJsonObject")]
        public async Task<ActionResult<GetJsonObjectResponse>> GetJsonObject(string bucketName, string fileName)
        {
            var response = await filesRepository.GetJsonObject(bucketName, fileName);

            return Ok(response);
        }
    }
}
