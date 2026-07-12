using Azure.Storage.Blobs;

namespace WebApplication1.Services
{
    public interface IBlobStorageService
    {
        Task<BlobContainerClient> GetBobContainerClient();

        Task<string> UploadBlob(IFormFile file, string imageName);
    }
}
