using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Awss3.Api.Contracts.Labels;
using Awss3.Api.Interfaces;

namespace Awss3.Api.Services
{
    public class LabelsService : ILabelsService
    {
        public async Task<IEnumerable<ImageLabelsResponse>> DetectLabels(string bucketName, string fileName)
        {
            var rekognitionClient = new AmazonRekognitionClient();

            var detectLabelsRequest = new DetectLabelsRequest
            {
                Image = new Image()
                {
                    S3Object = new S3Object()
                    {
                        Name = fileName,
                        Bucket = bucketName
                    }
                },
                MaxLabels = 10,
                MinConfidence = 75F
            };

            var detectLabelsResponse = await rekognitionClient.DetectLabelsAsync(detectLabelsRequest);

            var response = detectLabelsResponse.Labels?.Select(l => new ImageLabelsResponse()
            {
                LabelName = l.Name,
                ConfidenceValue = l.Confidence
            });

            return response ?? new List<ImageLabelsResponse>();
        }
    }
}
