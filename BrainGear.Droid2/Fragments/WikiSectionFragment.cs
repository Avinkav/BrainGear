using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Adapters;
using BrainGear.UI.Droid.Helpers;
using BrainGear.UI.Droid.ViewHolders;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;

namespace BrainGear.UI.Droid.Fragments
{
    public class WikiSectionFragment : GenericSectionFragment
    {
        private FloatingActionButton _editButton;
        private ViewPager _viewPager;
        private PagerAdapter _pagerAdapter;
        private TabLayout _tabLayout;
        private List<Fragment> _subFragments;
        public WikisViewModel Vm => App.Locator.Wiki;

        private List<Fragment> SubFragments
            => _subFragments ?? (_subFragments = new List<Fragment> {new WikiRelatedFragment()});

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _pagerAdapter = new GenericPagerAdapter(ChildFragmentManager, SubFragments);
            TitleResId = Resource.String.wiki_main_title;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.SectionWithTabs, container, false);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tablayout);
            _viewPager = view.FindViewById<ViewPager>(Resource.Id.pager);
            _viewPager.Adapter = _pagerAdapter;
            _tabLayout.PostDelayed(() => _tabLayout.SetupWithViewPager(_viewPager), 250);
            OnCreateView(inflater, container, savedInstanceState, view);
            return view;
        }
    }
}