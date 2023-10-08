using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
    }
}





















