using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace BrainGear.UI.Droid
{
    class CacheService
    {
       static string FILE_PATH = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public static async Task<bool> WriteCacheAsync<T>(T list, string fileName)
        {
            try
            {
                var output = JsonConvert.SerializeObject(list);
                string filePath = Path.Combine(FILE_PATH, fileName);
                using (var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                using (var strm = new StreamWriter(file))
                {
                    await strm.WriteAsync(output);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static async Task<T> ReadCacheAsync<T>(string fileName)
        {
            try
            {
                string input;
                string filePath = Path.Combine(FILE_PATH, fileName);
                using (var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                using (var strm = new StreamReader(file))
                {
                    input = await strm.ReadToEndAsync();
                }

                var list = JsonConvert.DeserializeObject<T>(input);
                return list;
            }
            catch
            {
                return default(T);
            }
        }
    }
}