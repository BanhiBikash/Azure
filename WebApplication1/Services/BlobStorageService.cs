using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace WebApplication1.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly string blobContainerName = "attendeeimages";

        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  async Task<BlobContainerClient> GetBobContainerClient()
        {
            try
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(_configuration["AzureStorageConnectionString"], blobContainerName);
                await blobContainerClient.CreateIfNotExistsAsync();
                return blobContainerClient;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
                throw new InvalidOperationException("Failed to create blob container");
            }
        }

        public async Task<string> UploadBlob(IFormFile file, string imageName)
        {
            try
            {
                string blobName = $"{imageName}{Path.GetExtension(file.FileName)}";
                var blobContainerClient = await GetBobContainerClient();
                var memoryStream = new MemoryStream();

                //close the stream as soon as the file is copied to the memory stream
                using (memoryStream) {
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    var client = await blobContainerClient.UploadBlobAsync(blobName, memoryStream);
                }

                return blobName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new InvalidOperationException("Failed to upload blob");
            }
        }

        public async Task<string> GetBlobUrl(string profileImage)
        {
            var container = await GetBobContainerClient();
            var blob = container.GetBlobClient(profileImage);

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container.Name,
                BlobName = blob.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(3)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            //get the blob url with the SAS token
            string blobName = blob.GenerateSasUri(sasBuilder).ToString();
            return blobName;
        }

        public async Task RemoveBlob(string profileImage)
        {
            var container = await GetBobContainerClient();
            var blob = container.GetBlobClient(profileImage);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
    }
}
