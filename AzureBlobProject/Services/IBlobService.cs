using AzureBlobProject.Models;

namespace AzureBlobProject.Services
{
    public interface IBlobService
    {
        Task<List<string>> GetAllBlobs(string containerName);

        Task <BlobModel> GetBlobWithUri(string containerName);

        Task <string> GetBlob(string name, string container);

        Task <bool> CreateBlob(string name, IFormFile file, string containderName, BlobModel blobModel);

        Task <bool> DeleteBlob(string name, string container);

    }
}
