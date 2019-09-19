using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ComputerVisionTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        // GET api/values
        [HttpPost]
        public async Task<ActionResult<string>> UploadImageAsync(IList<IFormFile> files)
        {
            IFormFile file = files[0];
            string response;

            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                response = await ComputerVisionService.Analyze(imageBytes);
            }

            return response;
        }


    }
}
