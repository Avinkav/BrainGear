using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.WindowsAzure.MobileServices;
using BrainGear.Data;
using BrainGear.UI.Droid.Adaptors;

namespace BrainGear.UI.Droid
{
    [Activity(Label = "TestDataActivity")]
    public class TestDataActivity : ListActivity
    {

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CurrentPlatform.Init();

            //List to hold results of following add operations
            List<Task> results = new List<Task>();
            // Adding Users
            var users = new UserList();
            var tableUser = AzureDataService.Client.GetTable<User>();
            users.ForEach(u => 
            {
               results.Add(
                   tableUser.InsertAsync(u)
                   );
            });

            // Adding videos
            var videos = new VideoList();
            var tableVideo = AzureDataService.Client.GetTable<Video>();
            videos.ForEach(v =>
            {
                results.Add(
                    tableVideo.InsertAsync(v)
                    );
            });

            // Adding  units
            var units = new UnitList();
            var tableUnits = AzureDataService.Client.GetTable<Unit>();
            units.ForEach(u =>
            {
                results.Add(
                    tableUnits.InsertAsync(u)
                    );
            });

            // Adding Units for Users. Just going to populate table with data for now. TODO: User object should contain units. Store data properly on backend
            var userUnitList = new UserUnitList();
            var tableUnitList = AzureDataService.Client.GetTable<UserUnit>();
            userUnitList.ForEach(u =>
                {
                    results.Add(
                        tableUnitList.InsertAsync(u)
                        );
                });


            RefreshUI(results);
            //ThreadPool.QueueUserWorkItem(o => DownloadVideo());


        }

        private async void RefreshUI(List<Task> results)
        {
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new List<string>());
            ListAdapter = adapter;
            while (true)
            {
                adapter.Clear();
                adapter.Add("Status");

                foreach (var result in results)
                {
                    adapter.Add(result.Status.ToString());
                }
                adapter.NotifyDataSetChanged();

                await Task.Delay(1000);
            }

            /*MobileServiceTable<Video> VideoTable = DataService.MobileService.GetTable<Video>();
            List<Video> items = await VideoTable.ToListAsync();
            RunOnUiThread(() => ListAdapter = new VideoAdapter(this, items));*/
        }
    }
}