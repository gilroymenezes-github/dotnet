using Awss3.Api.Contracts.Labels;
using Awss3.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Awss3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelsService labelsService;

        public LabelsController(ILabelsService labelsService)
        {
            this.labelsService = labelsService;
        }

        [HttpPost]
        [Route("{bucketName}/detect/{fileName}")]
        public async Task<ActionResult<IEnumerable<ImageLabelsResponse>>> DetectImageLabels(string bucketName, string fileName)
        {
            var response = await labelsService.DetectLabels(bucketName, fileName);

            if (response is null) return BadRequest();

            return Ok(response);
        }
    }
}
