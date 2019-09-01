
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services
{
    static class ComputerVisionService
    {
        static string subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
        static string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");

        static string uriBase = endpoint + "vision/v1.0/analyze";

        public static async Task<string> Analyze(byte[] image)
        {

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
    }
}