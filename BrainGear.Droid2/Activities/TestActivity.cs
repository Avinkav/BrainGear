using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using BrainGear.Data.DataServices;
using BrainGear.Data.Model;
using BrainGear.UI.Droid.Helpers;
using BrainGear.UI.Droid.ViewHolders;

namespace BrainGear.UI.Droid.Activities
{
    [Activity(Label = "TestActivity")]
    public class DataBridgeActivity : Activity
    {
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.List);

            var dataService = new AzureDataService();

            var video = new Video()
            {
                Id = Guid.NewGuid().ToString(),
                Description =
                    "Disruptive Innovation theory observes how new innovations create a new market and a new value network, which in turn disrupts an existing market. What often happens with companies that stay too close to their existing customers and invest aggressively to retain them, rather than investing to serve the needs of their future customers.",
                EmbedUrl = "Cu6J6taqOSg",
                Title = "Disruptive Innovation",
                Votes = 0,
                RelatedUnits = "IFB101",
                ThumbnailUrl = "http://img.youtube.com/vi/Cu6J6taqOSg/hqdefault.jpg",
                Tags = "Innovation,Disruptive Innovation,IFB101",
                Comments = new ObservableCollection<Comment>()
                {
                    new Comment()
                    {
                        CreatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid().ToString(),
                        Text = "Great Video!"
                    }
                }
            };

            var result = await dataService.SaveVideo(video);
            //result.ContinueWith((t) =>
            //{
            //    RunOnUiThread(() => Toast.MakeText(this, result.Status.ToString(), ToastLength.Long).Show());
            //});
        }
            

    }
}