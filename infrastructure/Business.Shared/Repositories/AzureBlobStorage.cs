using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IBlobStorage
    {
        Task<byte[]> DownloadBlobAsync(string blobName);
        Task<string> UploadBlobAsync(string blobName, string contentType, Stream fileStream);
    }

    public class AzureBlobStorage : IBlobStorage
    {
        string azureStorageAccountConnectionString;
        string azureStorageAccountContainerName;

        public AzureBlobStorage(IConfiguration configuration)
        {
            azureStorageAccountConnectionString = configuration["Azure:Storage:ConnectionString"];
            azureStorageAccountContainerName = configuration["Azure:Storage:ContainerName"];
        }

        public async Task<string> UploadBlobAsync(string blobName, string contentType, Stream fileStream)
        {
            var blobContainerClient = new BlobContainerClient(azureStorageAccountConnectionString, azureStorageAccountContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType,
                CacheControl = "public"
            };
            await blobClient.UploadAsync(fileStream, blobHttpHeaders);

            return blobClient.Uri.ToString();
        }

        public async Task<byte[]> DownloadBlobAsync(string blobName)
        {
            var blobContainerClient = new BlobContainerClient(azureStorageAccountConnectionString, azureStorageAccountContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            using var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;
            return stream.ToArray();
        }
    }
}
