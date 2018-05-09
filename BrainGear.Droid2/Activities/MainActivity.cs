using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Activities;
using BrainGear.UI.Droid.Fragments;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using ActionBar = Android.App.ActionBar;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Fragment = Android.Support.V4.App.Fragment;

namespace BrainGear.UI.Droid
{
    [Activity(Theme = "@style/PrimaryTheme", Label = "BrainGear", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private const int LoginRequest = 69;
        private TabLayout _tabLayout;
        private ActionBar _actionbar;
        private DrawerLayout _drawerLayout;
        private ActionBarDrawerToggle _drawerToggle;
        private FrameLayout _rootContainer;
        private int _currentMenuItem = 0;
        public MainViewModel Vm => App.Locator.Main;

        private MyNavigationService Navigator => (MyNavigationService)ServiceLocator.Current.GetInstance<INavigationService>();

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            // Set our view from the "Root" layout resource
            SetContentView(Resource.Layout.Root);
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);
            _rootContainer = FindViewById<FrameLayout>(Resource.Id.rootLayout);

            //Setup Nav Drawer
            var navigationView = FindViewById<NavigationView>(Resource.Id.navigationView);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            //Drawer toggle
            _drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, Resource.String.open_drawer,
                Resource.String.close_drawer);
            _drawerLayout.SetDrawerListener(_drawerToggle);
            
            // Init MVVMLight
            App.Init();
            Navigator.SetRoot(this);
            var userId = PreferenceManager.GetDefaultSharedPreferences(this).GetString("UserId", "notfound");
            if (userId == "notfound")
            {
                // Present Login
                StartActivityForResult(typeof(LoginActivity), LoginRequest);
            }
            else
            {
                // Hacks, hacks, hacks for now....
                App.Locator.Login.LoginId = userId;
                var userVm = await App.Locator.Login.LoginAndGetUser();
                if (userVm != null)
                {
                    SimpleIoc.Default.Register(() => userVm);
                    NavigateTo(Resource.Id.navHome, false);
                }
                else
                {
                    StartActivityForResult(typeof(LoginActivity), LoginRequest);
                }
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == LoginRequest)
               new Handler().Post(() => NavigateTo(Resource.Id.navHome, false));
        }

        private void NavigationView_NavigationItemSelected(object sender,
            NavigationView.NavigationItemSelectedEventArgs e)
        {
            e.MenuItem.SetChecked(true);
            //Snackbar.Make(_rootContainer, e.MenuItem.ItemId.ToString(), Snackbar.LengthLong).Show();
            NavigateTo(e.MenuItem.ItemId, true);
            _drawerLayout.CloseDrawer(GravityCompat.Start);
        }

        private void NavigateTo(int menuItemId, bool addTobackStack)
        {
            if (_currentMenuItem == menuItemId) return;
            _currentMenuItem = menuItemId;
            switch (menuItemId)
            {
                case Resource.Id.navHome:
                    SwapFragment(_rootContainer.Id, this, new NewsfeedFragment(), addTobackStack);
                    break;
                case Resource.Id.navVideos:
                    SwapFragment(_rootContainer.Id, this, new VideoSectionFragment(), addTobackStack);
                    break;
                case Resource.Id.navLeaderboard:
                    SwapFragment(_rootContainer.Id, this, new LeaderboardFragment(), addTobackStack);
                    break;
                case Resource.Id.navWiki:
                    SwapFragment(_rootContainer.Id, this, new WikiSectionFragment(), addTobackStack);
                    break;
                case Resource.Id.navProfile:
                    StartActivity(typeof (ProfileActivity));
                    break;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item) => _drawerToggle.OnOptionsItemSelected(item) || base.OnOptionsItemSelected(item);

        private void SwapFragment(int containerId, AppCompatActivity activity, Fragment fragment, bool addTobackStack)
        {
            var transaction = activity.SupportFragmentManager.BeginTransaction();
            if (addTobackStack) transaction.AddToBackStack(null);
            transaction.Replace(containerId, fragment).Commit();
            activity.SupportFragmentManager.ExecutePendingTransactions();
        }
    }


}
