namespace Awss3.Api.Contracts.Labels
{
    public class ImageLabelsRequest
    {
        public string? FileName { get;set; }
        public string? BucketName { get;set; }
        public int MaxLabelsCount { get; set; }
        public double MinConfidence { get; set; }
    }
}
