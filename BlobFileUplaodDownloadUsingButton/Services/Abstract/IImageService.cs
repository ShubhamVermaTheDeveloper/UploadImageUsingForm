namespace BlobFileUplaodDownloadUsingButton.Services.Abstract
{
    public interface IImageService
    {
        void UploadImageToAzure(IFormFile file);
    }
}
