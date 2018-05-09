using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace BrainGear.UI.Droid.ViewHolders
{
    public class LeaderboardViewHolder : RecyclerView.ViewHolder
    {
        public LeaderboardViewHolder(View itemView) : base(itemView)
        {
            TextName = itemView.FindViewById<TextView>(Resource.Id.text1);
            TextScore = itemView.FindViewById<TextView>(Resource.Id.text2);
            TextRank = itemView.FindViewById<TextView>(Resource.Id.text3);
        }

        public TextView TextName { get; set; }
        public TextView TextScore { get; set; }
        public TextView TextRank { get; set; }
    }
}