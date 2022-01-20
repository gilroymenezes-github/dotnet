using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Awss3.Api.Contracts.Files;
using Awss3.Api.Interfaces;
using System.Text.Json;

namespace Awss3.Api.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        private readonly IAmazonS3 s3client;

        public FilesRepository(IAmazonS3 s3client)
        {
            this.s3client = s3client;
        }

        public async Task<IEnumerable<GetFileListResponse>> ListFiles(string bucketName)
        {
            var response= await s3client.ListObjectsAsync(bucketName);

            return response.S3Objects.Select(b => new GetFileListResponse
            {
                BucketName = b.BucketName,
                Key = b.Key,
                Owner = b.Owner.DisplayName,
                Size = b.Size
            });
        }

        public async Task DownloadFile(string bucketName, string fileName)
        {
            var downloadLocalPath = Path.GetFullPath($"C:\\temp\\{fileName}");

            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = fileName,
                FilePath = downloadLocalPath
            };

            using var transferUtility = new TransferUtility(s3client);

            await transferUtility.DownloadAsync(downloadRequest);
        }

        public async Task<AddFileListResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles)
        {
            var response = new List<string>();
            
            foreach(var file in formFiles)
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = file.FileName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.NoACL
                };
                using var fileTransferUtility = new TransferUtility(s3client);
                
                await fileTransferUtility.UploadAsync(uploadRequest);
                
                var expiryUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    Expires = DateTime.UtcNow.AddDays(1)
                };
                var url = s3client.GetPreSignedURL(expiryUrlRequest);

                response.Add(url);
            }
            return new AddFileListResponse
            {
                PreSignedUrls = response
            };
        }

        public async Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName)
        {
            var multiObjectDeleteRequest = new DeleteObjectsRequest
            {
                BucketName = bucketName
            };
            multiObjectDeleteRequest.AddKey(fileName);

            var response = await s3client.DeleteObjectsAsync(multiObjectDeleteRequest);

            return new DeleteFileResponse
            {
                DeletedObjectsCount = response.DeletedObjects.Count
            };
        }

        public async Task AddJsonObject(string bucketName, AddJsonObjectRequest request)
        {
            var createdOnUtc = DateTime.UtcNow;

            var s3key = $"{createdOnUtc:yyyy}/{createdOnUtc:MM}/{createdOnUtc:dd}/{request.Id}";

            var putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = s3key,
                ContentBody = JsonSerializer.Serialize(request)
            };

            await s3client.PutObjectAsync(putObjectRequest);
        }

        public async Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };
            var response = await s3client.GetObjectAsync(request);

            using var reader = new StreamReader(response.ResponseStream);

            var contents = reader.ReadToEnd();

            return JsonSerializer.Deserialize<GetJsonObjectResponse>(contents) ?? new();
        }
    }
}
