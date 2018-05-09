using System.Collections.Generic;
using Android.Widget;

namespace BrainGear.UI.Droid.Adapters
{
    public abstract class GenericAdapter<T> : BaseAdapter<T>
    {
        public List<T> Items { get; set; }

        public void AddAll(IList<T> items)
        {
            var list = items as List<T>;
            Items.Clear();
            Items.AddRange(list);
            NotifyDataSetChanged();
        }

        
    }
}