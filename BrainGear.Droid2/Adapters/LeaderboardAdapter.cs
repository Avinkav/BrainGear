using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using BrainGear.Data.Model;

namespace BrainGear.UI.Droid.Adapters
{
    class LeaderboardAdapter : GenericAdapter<User>
    {
        Activity context;

        public LeaderboardAdapter(Activity context, List<User> user)
            : base()
        {
            this.context = context;
            this.Items = user;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override User this[int position]
        {
            get { return Items[position]; }
        }
        public override int Count
        {
            get { return Items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = Items[position].Name;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = Items[position].Score.ToString();
            return view;
        }


    }
}