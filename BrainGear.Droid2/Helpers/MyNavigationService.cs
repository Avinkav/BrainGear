using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Activities;
using GalaSoft.MvvmLight.Views;

namespace BrainGear.UI.Droid
{
    /// <summary>
    /// Custom NavigationService for the app. Call SetNavigationOptions in the main Activity before any ViewModels fire navigation methods.
    /// </summary>
    public class MyNavigationService : INavigationService
    {
        private Activity _rootActivity;
        private Stack<string> _navigationStack;
        private Dictionary<string, Type> _pages;
        private Dictionary<string, object> _parameters;


        public MyNavigationService()
        {
            _navigationStack = new Stack<string>();
            _pages = new Dictionary<string, Type>();
            _parameters = new Dictionary<string, object>();
        }

        public void GoBack()
        {
            _navigationStack.Pop();
            var previousKey = _navigationStack.Peek();
            NavigateTo(previousKey);
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            var intent = new Intent(_rootActivity, _pages[pageKey]);
            var key = Guid.NewGuid().ToString();
            _parameters.Add(key, parameter);
            intent.PutExtra(ParamKey, key);
            _rootActivity.StartActivity(intent);
            _navigationStack.Push(pageKey);
        }

        public void Configure(string pageKey, Type activityType)
        {
            if (activityType.IsSubclassOf(typeof(Activity)))
                _pages.Add(pageKey, activityType);
            else
            {
                throw new Exception("Type must be a subclass of Activity");
            }
        }

        /// <summary>
        /// Has to be called by the root activity before using this service for navigation
        /// </summary>
        /// <param name="rootActivity"></param>
        /// <param name="fragmentContainerId"></param>
        public void SetNavigationOptions(Activity rootActivity)
        {
            _rootActivity = rootActivity;
        }

        public string CurrentPageKey { get; private set; }
        public const string ParamKey = "ParameterKey";

        public T GetAndRemoveParameter<T>(string key)
        {
            var value = (T) _parameters[key];
            _parameters.Remove(key);
            return value;
        }

        public void SetRoot(Activity activity)
        {
            _rootActivity = activity;
        }
    }
}