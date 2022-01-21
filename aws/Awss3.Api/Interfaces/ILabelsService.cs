using Awss3.Api.Contracts.Labels;

namespace Awss3.Api.Interfaces
{
    public interface ILabelsService
    {
        Task<IEnumerable<ImageLabelsResponse>> DetectLabels(string bucketName, string fileName);
    }
}
