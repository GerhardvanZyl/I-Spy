
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Services
{
    static class ComputerVisionService
    {
        static string subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
        static string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");
        static string uriBase = endpoint + "vision/v1.0/analyze";
        static string maxImageSizeValue = Environment.GetEnvironmentVariable("MAX_IMAGE_SIZE");
        public static async Task<string> Analyze(byte[] image)
        {

            image = RightSizeImage(image);

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "visualFeatures=Categories,Description,Color";
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                using (ByteArrayContent content = new ByteArrayContent(image))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                    response = await client.PostAsync(uri, content);
                }

                string contentString = await response.Content.ReadAsStringAsync();

                Console.WriteLine(contentString);

                return contentString;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                throw;
            }
        }

        private static byte[] RightSizeImage(byte[] image)
        {
            float imageSize = image.Length;
            
            if(!float.TryParse(maxImageSizeValue, out float maxImageSize))
            {
                throw new ArgumentException("The MAX_IMAGE_SIZE environment variable should be a number, but was either the wrong type, or doesn't exist.");
            }

            if (imageSize > maxImageSize)
            {
                float ratio = maxImageSize / imageSize;
                using (Image<Rgba32> tempImg = Image.Load(image))
                {
                    tempImg.Mutate(x => x.Resize((int)(tempImg.Width * ratio), (int)(tempImg.Height * ratio)));
                    using (var ms = new MemoryStream())
                    {
                        tempImg.SaveAsJpeg(ms);
                        image = ms.ToArray();
                    }
                }
            }

            return image;
        }
    }
}