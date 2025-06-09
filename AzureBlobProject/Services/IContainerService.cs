namespace AzureBlobProject.Services
{
    public interface IContainerService
    {
        Task<List<string>> GetContainersAndBlobsAsync();
        Task<List<string>> GetAllContainerAsync();
        Task CreateContainersAsync(string containderName);
        Task DeleteContainersAsync(string containderName);
    }
}
