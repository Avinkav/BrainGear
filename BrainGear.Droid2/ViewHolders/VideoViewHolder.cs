using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace BrainGear.UI.Droid.ViewHolders
{
    class VideoViewHolder : RecyclerView.ViewHolder
    {
        public VideoViewHolder(View view, Action<object,int> T) : base(view)
        {
            TextTitle = view.FindViewById<TextView>(Resource.Id.textLine1);
            TextDescription = view.FindViewById<TextView>(Resource.Id.textLine2);
            ImageView = view.FindViewById<ImageView>(Resource.Id.imageView1);
            view.Click += (sender, e) => T(this, LayoutPosition);
        }

        public TextView TextTitle { get; set; }

        public TextView TextDescription { get; set; }

        public ImageView ImageView { get; set; }
    }
}