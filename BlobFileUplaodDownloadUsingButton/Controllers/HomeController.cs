using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BlobFileUplaodDownloadUsingButton.Models;
using BlobFileUplaodDownloadUsingButton.Services.Abstract;
using BlobFileUplaodDownloadUsingButton.Services.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlobFileUplaodDownloadUsingButton.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;

        public HomeController(ILogger<HomeController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;

        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePicture(ImageModel imageModel)
        {

            string fileExtenction = Path.GetExtension(imageModel.File.FileName);
            if(imageModel.File == null || imageModel.File.FileName == null)
                return View("Index");

            _imageService.UploadImageToAzure(imageModel.File);
            return View("Index");
        }



        public IActionResult PreviewImages()
        {
            ViewBag.ImageUrl = _imageService.GetImages();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}