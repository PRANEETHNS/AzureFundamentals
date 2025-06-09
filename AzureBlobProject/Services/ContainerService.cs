
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobProject.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobClient;

        public ContainerService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }

        public async Task CreateContainersAsync(string containderName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containderName);

            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);           
        }

        public async Task DeleteContainersAsync(string containderName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containderName);
            await blobContainerClient.DeleteIfExistsAsync();
            
        }

        public async Task<List<string>> GetAllContainerAsync()
        {
            List<string> containerName = new List<string>();

            await foreach (BlobContainerItem blobItem in _blobClient.GetBlobContainersAsync())
            {
                containerName.Add(blobItem.Name);
            }   

            return containerName;          
        }

        public Task<List<string>> GetContainersAndBlobsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
