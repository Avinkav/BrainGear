using System;
using Android.Content.Res;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace BrainGear.UI.Droid.ViewHolders
{
    public class WikiViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextTitle;
        public TextView TextContent;

        public WikiViewHolder(View view, Action<object, int> onClick) : base(view)
        {
            TextTitle = view.FindViewById<TextView>(Resource.Id.text1);
            TextContent = view.FindViewById<TextView>(Resource.Id.text2);
            CardView = view.FindViewById<CardView>(Resource.Id.cardView);
            TextContent.SetLines(10);
            TextContent.Ellipsize = TextUtils.TruncateAt.End;

            CardView.Click += (sender, args) => onClick(sender, AdapterPosition);
        }

        public CardView CardView { get; set; }
    }
}