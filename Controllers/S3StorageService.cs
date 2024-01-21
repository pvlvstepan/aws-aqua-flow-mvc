using Amazon.S3.Model;
using Amazon.S3;
using Amazon;

namespace AquaFlow.Controllers
{
    public class S3StorageService : IStorageService
    {
        private readonly AmazonS3Client s3Client;
        private const string BUCKET_NAME = "aquaflow-bucket";
        private const string FOLDER_NAME = "assets";
        private const double DURATION = 24;
        public S3StorageService()
        {
            s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
        }

        public async Task<string> AddItem(IFormFile file, string readerName)
        {
            string fileName = file.FileName;
            string objectKey = $"{FOLDER_NAME}/{readerName}/{fileName}";
            using (Stream fileToUpload = file.OpenReadStream())
            {
                var putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = BUCKET_NAME;
                putObjectRequest.Key = objectKey;
                putObjectRequest.InputStream = fileToUpload;
                putObjectRequest.ContentType = file.ContentType;

                var response = await s3Client.PutObjectAsync(putObjectRequest);
                return GeneratePreSignedURL(objectKey);
            }
        }

        public string GeneratePreSignedURL(string objectKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = BUCKET_NAME,
                Key = objectKey,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(DURATION)
            };

            string url = s3Client.GetPreSignedURL(request);
            return url;
        }

        public async Task<byte[]> GetItem(string keyName)
        {
            GetObjectResponse response = await s3Client.GetObjectAsync(BUCKET_NAME, keyName);
            MemoryStream memoryStream = new MemoryStream();

            using Stream responseStream = response.ResponseStream;
            responseStream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
