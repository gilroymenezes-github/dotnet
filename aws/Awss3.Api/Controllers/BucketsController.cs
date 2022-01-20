using Awss3.Api.Contracts.Buckets;
using Awss3.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Awss3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketsController : ControllerBase
    {
        private readonly IBucketsRepository bucketsRepository;

        public BucketsController(IBucketsRepository bucketsRepository)
        {
            this.bucketsRepository = bucketsRepository;
        }

        [HttpPost]
        [Route("create/{bucketName}")]
        public async Task<ActionResult<CreateBucketResponse>> CreateBucket([FromRoute] string bucketName)
        {
            var result = await bucketsRepository.CreateBucket(bucketName);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<IEnumerable<GetBucketListResponse>>> ListBuckets()
        {
            var result = await bucketsRepository.ListBuckets();

            if (result is null) return NotFound();

            return Ok(result);
        }
         
        [HttpDelete]
        [Route("delete/{bucketName}")]
        public async Task<IActionResult> DeleteBucket(string bucketName)
        {
            await bucketsRepository.DeleteBucket(bucketName);

            return Ok();
        }
    }
}
