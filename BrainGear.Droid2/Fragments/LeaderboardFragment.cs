using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Helpers;
using BrainGear.UI.Droid.ViewHolders;
using Fragment = Android.Support.V4.App.Fragment;

namespace BrainGear.UI.Droid.Fragments
{
    public class LeaderboardFragment : GenericSectionFragment
    {
        private RecyclerView _recyclerview;

        public RecyclerView Recyclerview
            => _recyclerview ?? (_recyclerview = View.FindViewById<RecyclerView>(Resource.Id.recyclerView));

        public LeaderboardViewModel Vm => App.Locator.Leaderboard;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            TitleResId = Resource.String.leaderboard_main_title;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PageLeaderboard, container, false);
            Toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            _recyclerview = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            Recyclerview.SetLayoutManager(new LinearLayoutManager(Activity));
            Recyclerview.SetAdapter(Vm.Model.GetRecyclerAdapter(OnBind, OnCreate));
            return view;
        }

        private void OnBind(RecyclerView.ViewHolder viewHolder, int position, Leaderboard item)
        {
            var vh = viewHolder as LeaderboardViewHolder;
            vh.TextName.Text = item.UserName;
            vh.TextScore.Text = item.Score.ToString();
            vh.TextRank.Text = (position + 1).ToString();
        }

        private RecyclerView.ViewHolder OnCreate(ViewGroup parent, int itemType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemLeaderboard, parent, false);
            return new LeaderboardViewHolder(view);
        }

        public override void OnStart()
        {
            Vm.RefreshCommand.Execute(null);
            base.OnStart();
        }
    }
}