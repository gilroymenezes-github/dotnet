namespace Awss3.Api.Contracts.Files
{
    public class GetFileListResponse
    {
        public string? BucketName { get; set; }
        public string? Key { get; set; }
        public string? Owner { get; set; }
        public long Size { get; set; }
    }
}
