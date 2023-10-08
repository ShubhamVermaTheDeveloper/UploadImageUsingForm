using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using BlobFileUplaodDownloadUsingButton.Options;
using BlobFileUplaodDownloadUsingButton.Services.Abstract;
using Microsoft.Extensions.Options;

namespace BlobFileUplaodDownloadUsingButton.Services.Concrete
{
    public class ImageService : IImageService
    {
        private readonly AzureOptions _azureOptions;    

        public ImageService(IOptions<AzureOptions> azureOptions) 
        { 
            _azureOptions = azureOptions.Value;
        }

        public void UploadImageToAzure(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);

            using MemoryStream fileUploadStream = new MemoryStream();
            file.CopyTo(fileUploadStream);
            fileUploadStream.Position = 0;
            BlobContainerClient blobContainerClient = new BlobContainerClient(
                _azureOptions.ConnectionString,
                _azureOptions.Container
                );
            var uniqueName = Guid.NewGuid().ToString() + fileExtension;
            BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueName);

            blobClient.Upload(fileUploadStream, new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/bitmap"
                }
            }, cancellationToken: default);
        }


        public List<string> GetImages()
        {
            BlobServiceClient _blobServiceClient = new BlobServiceClient(_azureOptions.ConnectionString);
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_azureOptions.Container);
            var blobs = blobContainerClient.GetBlobs();
            List<string> list = new List<string>();
            foreach (var blobItem in blobs)
            {
                list.Add(GetURL(blobItem.Name));
            }
            return list;
        }


        public string GetURL(string blobName)
        {
            string sasToken = GetSasToken(_azureOptions.ConnectionString, _azureOptions.Container, blobName, BlobSasPermissions.Read);
            var blobServiceClient = new BlobServiceClient(new Uri(new BlobServiceClient(_azureOptions.ConnectionString).GetBlobContainerClient(_azureOptions.Container).GetBlobClient(blobName).Uri.ToString() + sasToken));
            return blobServiceClient.Uri.ToString();
        }


        public string GetSasToken(string connectionString, string containerName, string blobName, BlobSasPermissions permissions)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var builder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTime.UtcNow.AddMinutes(-5),
                ExpiresOn = DateTime.UtcNow.AddHours(1),
            };

            builder.SetPermissions(permissions);
            return blobClient.GenerateSasUri(builder).Query;
        }

        
    }
}





















