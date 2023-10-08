using Azure.Storage.Sas;

namespace BlobFileUplaodDownloadUsingButton.Services.Abstract
{
    public interface IImageService
    {
        void UploadImageToAzure(IFormFile file);
        string GetSasToken(string connectionString, string containerName, string blobName, BlobSasPermissions permissions);
        string GetURL(string blobName);
        List<string> GetImages();
    }
}
