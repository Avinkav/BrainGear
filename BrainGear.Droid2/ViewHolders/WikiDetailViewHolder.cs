using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace BrainGear.UI.Droid.ViewHolders
{
    public class WikiDetailViewHolder : RecyclerView.ViewHolder
    {
        public WikiDetailViewHolder(View view) : base(view)
        {
            TitleText = view.FindViewById<TextView>(Resource.Id.text1);
            ContentText = view.FindViewById<TextView>(Resource.Id.text2);
        }

        public TextView ContentText { get; set; }

        public TextView TitleText { get; set; }
    }
}