
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

        public async Task<List<string>> GetContainersAndBlobsAsync()
        {
            List<string> containerAndBlobName = new List<string>();
            containerAndBlobName.Add("------- Account Name:"+ _blobClient.AccountName+ " -------");
            containerAndBlobName.Add("----------------------------------------------------------");
            await foreach (BlobContainerItem blobItem in _blobClient.GetBlobContainersAsync())
            {
                containerAndBlobName.Add("+++++++++"+blobItem.Name);
				BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(blobItem.Name);

                await foreach (BlobItem item in blobContainerClient.GetBlobsAsync())
                {
					//get metadata
					var blobClient = blobContainerClient.GetBlobClient(item.Name);
                    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();

                    string tempBlobToAdd = blobItem.Name;

                    if (blobProperties.Metadata.ContainsKey("title"))
                    {
                        tempBlobToAdd+="(" + blobProperties.Metadata["title"]+")";
                    }

					containerAndBlobName.Add( "------" + tempBlobToAdd);
				}
				containerAndBlobName.Add("------------------------------------------------------------------------------------------------------------");
			}

            return containerAndBlobName;
        }
    }
}
