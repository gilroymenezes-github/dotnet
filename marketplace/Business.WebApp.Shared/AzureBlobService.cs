using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.WebApp.Shared
{
    public class AzureBlobService
    {
        string azureStorageAccountConnectionString;
        string azureStorageAccountContainerName;
        
        public AzureBlobService(IConfiguration configuration)
        {
            azureStorageAccountConnectionString = configuration["Azure:Storage:ConnectionString"];
            azureStorageAccountContainerName = configuration["Azure:Storage:ContainerName"];
        }

        public async Task<string> UploadBlobAsync(string blobName, Stream fileStream)
        {
            var blobContainerClient = new BlobContainerClient(azureStorageAccountConnectionString, azureStorageAccountContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = "application/pdf",
                CacheControl = "public"
            };

            await blobClient.UploadAsync(fileStream, blobHttpHeaders);

            return blobClient.Uri.ToString();
        }
    }
}
