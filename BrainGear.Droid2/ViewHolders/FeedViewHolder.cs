using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace BrainGear.UI.Droid.ViewHolders
{
    public class FeedViewHolder : RecyclerView.ViewHolder
    {
        public FeedViewHolder(View view, Action<object, int> onItemClick) : base(view)
        {
            TextTitle = view.FindViewById<TextView>(Resource.Id.textTitle);
            TextUnit = view.FindViewById<TextView>(Resource.Id.textUnit);
            TextDescription = view.FindViewById<TextView>(Resource.Id.textDescription);
            ImageView = view.FindViewById<ImageView>(Resource.Id.imageView1);
            CardView = view.FindViewById<CardView>(Resource.Id.cardView);
            view.Click += (sender, e) => onItemClick(this, AdapterPosition);
        }

        public TextView TextTitle { get; private set; }

        public TextView TextDescription { get; private set; }

        public TextView TextUnit { get; private set; }

        public ImageView ImageView { get; private set; }

        public CardView CardView { get; private set; }
    }
}