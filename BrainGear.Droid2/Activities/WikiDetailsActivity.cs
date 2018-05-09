using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Fragments;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace BrainGear.UI.Droid.Activities
{
    [Activity(Label = "Wiki Page", LaunchMode = LaunchMode.SingleTop, Theme = "@style/PrimaryTheme")]
    public class WikiDetailsActivity : AppCompatActivity
    {
        private Binding<string, string> _titleBinding;
        private Toolbar _toolbar;
        private FloatingActionButton _fab;
        public WikiViewModel Vm { get; private set; }

        private MyNavigationService _navigator
            => (MyNavigationService) ServiceLocator.Current.GetInstance<INavigationService>();

        public FloatingActionButton FloatingButton => _fab;

        private EditWikiFragment _editFragment;
        private WikiReadFragment _readFragment;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            SetContentView(Resource.Layout.PageWikiDetails);

            _toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = "Wiki";
            var key = Intent.GetStringExtra(MyNavigationService.ParamKey);
            Vm = _navigator.GetAndRemoveParameter<WikiViewModel>(key);
            Vm.RefreshCommand.Execute(null);
            _editFragment = new EditWikiFragment() { Vm = Vm };
            _readFragment = new WikiReadFragment { Vm = Vm };


            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.fragmentContainer, _readFragment)
                .Commit();

            _fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            _fab.Click += _fab_Click;

        }

        private void _fab_Click(object sender, System.EventArgs e)
        {
            _editFragment.CommitEdits();
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.fragmentContainer, _readFragment)
                .Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.WikiDetailsMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.edit:
                    EditWiki();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void EditWiki()
        {
            // Insert Fragment
            SupportFragmentManager.BeginTransaction()
                .AddToBackStack(null)
                .Replace(Resource.Id.fragmentContainer, _editFragment )
                .Commit();
            _fab.Show();
        }
 
    }
}