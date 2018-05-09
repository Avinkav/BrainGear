using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using BrainGear.Data;

namespace BrainGear.UI.Droid
{
    public class AuthHelper
    {
        public static async System.Threading.Tasks.Task Authenticate(Activity activity)
        {
            try
            {
                //AzureDataService.User = await AzureDataService.Client.LoginAsync(activity, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
            }
            catch (Exception ex)
            {
                Toast.MakeText(activity, ex.Message, ToastLength.Long).Show();
            }
        }
    }
}