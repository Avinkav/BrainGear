using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;

namespace BrainGear.Data
{
    public class ImageService
    {
        public async static Task<string> UploadProfilePic(string userId, byte[] image)
        {
            var param = new Dictionary<string,string>{
                { "containerName" , "profile" },
                { "blobName" , userId + ".jpeg" }
            };
            var sasRequest = await AzureDataService.Client.InvokeApiAsync("images", HttpMethod.Post, param );
            var sasURL = sasRequest.First.ToString();
            sasURL = sasRequest.First.First.Value<string>();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, sasURL);
                request.Content = new ByteArrayContent(image);
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                request.Content.Headers.ContentLength = image.Length;

                var sendTask = client.SendAsync(request);
                sendTask.Wait();

                return sendTask.Result.StatusCode.ToString();
            }
        }
    }
}
