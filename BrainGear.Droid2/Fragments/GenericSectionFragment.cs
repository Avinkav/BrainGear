using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace BrainGear.UI.Droid.Fragments
{
    public class GenericSectionFragment : Fragment
    {
        protected Toolbar Toolbar;

        internal void OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState, View view)
        {
            Toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
        }

        public override void OnResume()
        {
            base.OnResume();
            if (Toolbar == null) return;
            ((AppCompatActivity)Activity).SetSupportActionBar(Toolbar);
            ((AppCompatActivity)Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_white_18dp);
            ((AppCompatActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            if (TitleResId > 0)
                ((AppCompatActivity)Activity).SupportActionBar.SetTitle(TitleResId);
        }

        protected int TitleResId { get; set; }
    }
}