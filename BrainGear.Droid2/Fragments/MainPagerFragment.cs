using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Adapters;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace BrainGear.UI.Droid.Fragments
{
    public class MainPagerFragment : Android.Support.V4.App.Fragment
    {
        private VideosPagerAdapter _pagerAdapter;
        private ViewPager _viewPager;
        private TabLayout _tabLayout;
        public MainViewModel Vm => App.Locator.Main;

        private MyNavigationService Navigator => (MyNavigationService)ServiceLocator.Current.GetInstance<INavigationService>();

        public ViewPager ViewPager => _viewPager;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SectionVideo, container, false);

            // Init pager adapter and bind to ViewPager from the layout.
            _pagerAdapter = new VideosPagerAdapter(FragmentManager);
            _viewPager = view.FindViewById<ViewPager>(Resource.Id.pager);
            _viewPager.Adapter = _pagerAdapter;

            //Setup Tabs
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tablayout);
            //_tabLayout.SetupWithViewPager(ViewPager);
            Navigator.SetViewPager(ViewPager);
            return view;
        }

        #region Helper Methods

        #endregion
    }
}