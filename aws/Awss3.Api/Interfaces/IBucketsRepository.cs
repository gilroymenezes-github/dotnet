using Awss3.Api.Contracts.Buckets;

namespace Awss3.Api.Interfaces
{
    public interface IBucketsRepository
    {
        Task<bool> DoesBucketExist(string bucketName);
        Task<CreateBucketResponse> CreateBucket(string bucketName);
        Task<IEnumerable<GetBucketListResponse>> ListBuckets();
        Task DeleteBucket(string bucketName);
    }
}
