using Android.Widget;
using System;
using System.Threading.Tasks;

namespace BrainGear.UI.Droid
{
    public class MyDialogService : GalaSoft.MvvmLight.Views.IDialogService
    {
        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            Console.WriteLine(error.StackTrace);
            return ShowError(error.Message, title, buttonText, afterHideCallback);
        }

        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            return ShowMessage(message, title);
        }

        public Task ShowMessage(string message, string title)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
            return Task.Run(() =>
            {
                return true;
            });
        }

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            return ShowMessage(message, title);
        }

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            ShowMessage(message, title);
            return Task.Run(() =>
            {
                return true;
            });
        }

        public Task ShowMessageBox(string message, string title)
        {
            return ShowMessage(message, title);
        }
    }
}