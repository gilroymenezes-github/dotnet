using Awss3.Api.Contracts.Files;

namespace Awss3.Api.Interfaces
{
    public interface IFilesRepository
    {
        Task<AddFileListResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles);
        Task<IEnumerable<GetFileListResponse>> ListFiles(string bucketName);
        Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName);
        Task DownloadFile(string bucketName, string fileName);
        Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName);
        Task AddJsonObject(string bucketName, AddJsonObjectRequest request);
    }
}
