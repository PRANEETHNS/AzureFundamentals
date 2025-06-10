using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
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

        public async Task<List<BlobModel>> GetBlobWithUri(string containerName)
        {
			BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
			var blobs = blobContainerClient.GetBlobsAsync();

			List<BlobModel> blobList = new List<BlobModel>();
			await foreach (var blob in blobs)
			{
                var blobClient = blobContainerClient.GetBlockBlobClient(blob.Name);
                BlobModel blobModel = new BlobModel() { 
                    Uri = blobClient.Uri.AbsoluteUri
                    
                };

                if (blobClient.CanGenerateSasUri)
                {
                    BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                        BlobName = blob.Name,
                        Resource = "b",
                        ExpiresOn = DateTime.UtcNow.AddHours(1)
                    };

                    blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

                    blobModel.Uri = blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;

                }
                

                BlobProperties blobProperties = await blobClient.GetPropertiesAsync();

                if (blobProperties.Metadata.ContainsKey("title")) 
                {
					blobModel.Title = blobProperties.Metadata["title"];
				}
				if (blobProperties.Metadata.ContainsKey("comment"))
				{
					blobModel.Comment = blobProperties.Metadata["comment"];
				}
                blobList.Add(blobModel);
			}

			return blobList;
		}
    }
}
