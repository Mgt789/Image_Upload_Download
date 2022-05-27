using Microsoft.AspNetCore.Mvc;

namespace ImageDemo.Controllers
{
    public class GalleryController : Controller
    {
        //path declare
        private readonly string wwwrootDirectory =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");



        //Display the file
        public IActionResult Index()
        {
            //only png file upload
            List<string?> images = Directory.GetFiles(wwwrootDirectory, "*.png")
                .Select(Path.GetFileName)
                .ToList();

            return View(images);
        }

        [HttpPost] /*upload image from here*/
        public async Task<IActionResult> Index(IFormFile myFile)
        {
            if (myFile != null)
            {
                //path creatation
                var path = Path.Combine(
                    wwwrootDirectory,
                    DateTime.Now.Ticks.ToString() + Path.GetExtension(myFile.FileName));

                //save the file
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await myFile.CopyToAsync(stream);
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        //download code/Download Action method
        public async Task<IActionResult> DownloadFile(string filePath)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",filePath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(path);

            return File(memory,contentType,fileName);
        }
    }
}
