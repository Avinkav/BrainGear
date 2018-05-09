using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Adapters;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace BrainGear.UI.Droid.Fragments
{
    public class VideoSectionFragment : GenericSectionFragment
    {
        private VideosPagerAdapter _pagerAdapter;
        private TabLayout _tabLayout;
        private ViewPager _viewPager;

        public VideosViewModel Vm => App.Locator.Videos;

        public override void OnResume()
        {
            base.OnResume();
            _viewPager.SetCurrentItem(1, false);           
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.sample_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.refresh:
                    Vm.RefreshCommand.Execute(null);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            TitleResId = Resource.String.video_main_title;
            HasOptionsMenu = true;
            _pagerAdapter = new VideosPagerAdapter(ChildFragmentManager);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.SectionWithTabs, container, false);
            _viewPager = view.FindViewById<ViewPager>(Resource.Id.pager);
            _viewPager.Adapter = _pagerAdapter;
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tablayout);
            _tabLayout.PostDelayed(() => _tabLayout.SetupWithViewPager(_viewPager), 250);  // Setup tabs after everything has loaded or tabs does not inflate properly.
            OnCreateView(inflater, container, savedInstanceState, view);
            return view;
        }
    }
}