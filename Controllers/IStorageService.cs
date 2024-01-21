namespace AquaFlow.Controllers
{
    public interface IStorageService
    {
        Task<string> AddItem(IFormFile file, string readerName);
        Task<byte[]> GetItem(string objectKey);
        string GeneratePreSignedURL(string objectKey);
    }
}
