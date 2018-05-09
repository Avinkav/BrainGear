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
using BrainGear.Data.ViewModel;
using BrainGear.Data;
using BrainGear.UI.Droid.Activities;
using BrainGear.UI.Droid.Fragments;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace BrainGear.UI.Droid
{
    public static class App
    {
        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator
        {
            get
            {
                if (_locator != null)
                    return _locator;

                Init();
                return _locator;
            }
        }

        public static void Init()
        {
            if (_locator != null) return;
            var nav = new MyNavigationService();
            nav.Configure(ViewModelLocator.VIDEO_DETAILS, typeof(VideoPlayerActivity));
            nav.Configure(ViewModelLocator.WIKI_DETAILS, typeof(WikiDetailsActivity));

            SimpleIoc.Default.Register<INavigationService>(() => nav);

            SimpleIoc.Default.Register<IDialogService, MyDialogService>();
            _locator = new ViewModelLocator();
        }
    } 
}