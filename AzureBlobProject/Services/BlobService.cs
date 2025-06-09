using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Models;
using System.ComponentModel;
using System.Xml.Linq;

namespace AzureBlobProject.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;

        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }


        public async Task <bool> CreateBlob(string name, IFormFile file, string container, BlobModel blobModel)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(container);
            var blobClient = blobContainerClient.GetBlobClient(name);

            var HttpHeader = new Azure.Storage.Blobs.Models.BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            var result = await blobClient.UploadAsync(file.OpenReadStream(), HttpHeader);

            if (result != null) {
                return true;
            }
            else return false;

        }

        public async Task<bool> DeleteBlob(string name, string container)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(container);
            var blobClient = blobContainerClient.GetBlobClient(name);

            return await blobClient.DeleteIfExistsAsync();

        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();

            List<string> blobNames = new List<string>();
            await foreach (var blob in blobs)
            {
                blobNames.Add(blob.Name);
            }

            return blobNames;
        }

        public async Task<string> GetBlob(string name, string container)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(container);
            var blobClient = blobContainerClient.GetBlobClient(name);

            if (blobClient != null) 
            {
                return blobClient.Uri.AbsoluteUri;
            }

            return "";
        }

        public async Task<BlobModel> GetBlobWithUri(string containerName)
        {
            throw new NotImplementedException();
            //BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            //var blobClient = blobContainerClient.GetBlobClient(name);
        }
    }
}
