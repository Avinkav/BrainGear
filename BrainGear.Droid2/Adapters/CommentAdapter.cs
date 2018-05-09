using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using BrainGear.Data.Model;

namespace BrainGear.UI.Droid.Adapters
{
    class CommentAdapter : GenericAdapter<Comment>
    {
        Activity context;

        public CommentAdapter(Activity context)
            : base()
        {
            this.context = context;
            this.Items = new List<Comment>();
        }

        public CommentAdapter(Activity context, List<Comment> items)
            : base()
        {
            this.context = context;
            this.Items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Comment this[int position]
        {
            get { return Items[position]; }
        }
        public override int Count
        {
            get { return Items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Items[position];
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Resource.Layout.ItemComment, parent, false);
            view.FindViewById<TextView>(Resource.Id.text1).Text = item.Text;
            view.FindViewById<TextView>(Resource.Id.text2).Text = item.UserName;

            return view;
        }



    }
}