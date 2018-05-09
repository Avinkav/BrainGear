using System;
using System.Collections.ObjectModel;
using Android.Support.V7.Widget;
using Android.Views;

namespace BrainGear.UI.Droid.Helpers
{
    public static class MyExtensions
    {
        public static ObservableRecylcerAdapter<T> GetRecyclerAdapter<T>(this ObservableCollection<T> collection, Action<RecyclerView.ViewHolder, int, T> OnBindFunc, Func<ViewGroup, int, RecyclerView.ViewHolder> OnCreateFunc)
        {
            return new ObservableRecylcerAdapter<T>
            {
                DataSource = collection,
                BindFunc = OnBindFunc,
                CreateFunc = OnCreateFunc
            };
        }
    }
}