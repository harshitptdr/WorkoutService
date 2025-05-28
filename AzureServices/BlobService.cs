using Azure.Storage.Blobs;
using WorkoutService.Logging;

namespace WorkoutService.AzureServices
{
    public class BlobService : IBlobService
    {

        private readonly Logger _logger;
        private readonly IConfiguration _config;

        public BlobService(Logger logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
    
        public async Task<string?> UploadProgressImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0) return null;

            var containerName = _config["AzureStorage:ContainerName"];
            var connectionString = _config["AzureStorage:ConnectionString"];

            // Create container client
            var containerClient = new BlobContainerClient(connectionString, containerName);
            await containerClient.CreateIfNotExistsAsync();
            //await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob); // Optional: for public access

            // Generate a unique name
            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);

            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(fileName);

            // Upload file stream
            await using var stream = image.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return fileName; // ✅ Only returning the filename
        }

    }
}
