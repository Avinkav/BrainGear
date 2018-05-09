using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Helpers;
using BrainGear.UI.Droid.ViewHolders;
using GalaSoft.MvvmLight.Helpers;
using Square.Picasso;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace BrainGear.UI.Droid.Fragments
{
    public class NewsfeedFragment : Fragment
    {
        private RecyclerView _recyclerView;
        private SwipeRefreshLayout _swipeContainer;
        private Binding<bool, bool> _refreshBinding;

        public NewsfeedViewModel Vm => App.Locator.Newsfeed;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = View.FindViewById<RecyclerView>(Resource.Id.listView));

        public SwipeRefreshLayout SwipeContainer => _swipeContainer ?? (_swipeContainer = View.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeContainer));

        public override void OnStart()
        {
            Vm.RefreshCommand.Execute(null);
            base.OnStart();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.PageNewsfeed, container, false);
            _swipeContainer = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeContainer);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            var layout = new LinearLayoutManager(Activity);
            _recyclerView.SetLayoutManager(layout);

            //Binding
            RecyclerView.SetAdapter(Vm.Model.GetRecyclerAdapter(OnBindFunc, OnCreateFunc));
            _refreshBinding = this.SetBinding(() => Vm.IsRefreshing, () => SwipeContainer.Refreshing);
            SwipeContainer.SetCommand("Refresh", Vm.RefreshCommand);
            return view;
        }

        public override void OnResume()
        {
            //Setup actionbar
            var toolbar = View.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ((AppCompatActivity)Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_white_18dp);
            ((AppCompatActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ((AppCompatActivity)Activity).SupportActionBar.SetTitle(Resource.String.newsfeed_title);

            base.OnResume();
        }

        private RecyclerView.ViewHolder OnCreateFunc(ViewGroup parent, int itemType)
        {
            var view = LayoutInflater.From(parent.Context)
                   .Inflate(Resource.Layout.ItemFeed, parent, false);
            return new FeedViewHolder(view, OnItemClick); ;
        }

        private void OnBindFunc(RecyclerView.ViewHolder viewHolder, int position, IFeedItemViewModel feedItem)
        {
            var vh = viewHolder as FeedViewHolder;
            if (feedItem.Type == typeof (VideoViewModel))
            {
                var vm = feedItem as VideoViewModel;
                vh.TextTitle.Text = vm.Model.Title;
                //string.Format(Resources.GetString(Resource.String.Related), video.RelatedUnits, 30);
                vh.TextDescription.Text = vm.Model.Description;

                if (vm.Model.ThumbnailUrl != "")
                    Picasso.With(Activity)
                        .Load(vm.Model.ThumbnailUrl)
                        .Into(vh.ImageView);
            }else if (feedItem.Type == typeof (WikiViewModel))
            {
                var vm = feedItem as WikiViewModel;
                vh.TextTitle.Text = vm.Model.Title;
                vh.TextDescription.Text = vm.Model.Summary;

                if (vm.Model.ThumbnailUrl != "")
                    Picasso.With(Activity)
                        .Load(vm.Model.ThumbnailUrl)
                        .Into(vh.ImageView);
            }
        }

        public void OnItemClick(object sender, int position)
        {
            Vm.ShowCommand.Execute(Vm.Model[position]);
        }
    }
}