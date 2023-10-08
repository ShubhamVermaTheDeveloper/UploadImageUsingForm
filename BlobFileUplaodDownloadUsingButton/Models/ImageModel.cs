using System.ComponentModel;

namespace BlobFileUplaodDownloadUsingButton.Models
{
    public class ImageModel
    {
        [DisplayName("Uplaod Image")]
        public string FileDetails { get; set; }

        public IFormFile? File { get; set; } 

    }
}
