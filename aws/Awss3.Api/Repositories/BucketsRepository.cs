using Amazon.S3;
using Amazon.S3.Model;
using Awss3.Api.Contracts.Buckets;
using Awss3.Api.Interfaces;

namespace Awss3.Api.Repositories
{
    public class BucketsRepository : IBucketsRepository
    {
        private readonly IAmazonS3 s3client;

        public BucketsRepository(IAmazonS3 s3client)
        {
            this.s3client = s3client;
        }

        public async Task<bool> DoesBucketExist(string bucketName)
        {
            return await s3client.DoesS3BucketExistAsync(bucketName); // pointless because it checks globally and will most likely return true
        }

        public async Task<CreateBucketResponse> CreateBucket(string bucketName)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };
            var response = await s3client.PutBucketAsync(putBucketRequest);

            return new CreateBucketResponse
            {
                BucketName = bucketName,
                RequestId = response.ResponseMetadata.RequestId
            };
        }

        public async Task<IEnumerable<GetBucketListResponse>> ListBuckets()
        {
            var response = await s3client.ListBucketsAsync();
            return response.Buckets.Select(b => new GetBucketListResponse
            {
                BucketName = b.BucketName,
                CreationDate = b.CreationDate
            });
        }

        public async Task DeleteBucket(string bucketName) =>  await s3client.DeleteBucketAsync(bucketName);
    }
}
