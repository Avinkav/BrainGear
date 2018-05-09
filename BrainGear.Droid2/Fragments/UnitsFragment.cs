using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Adapters;

namespace BrainGear.UI.Droid.Fragments
{
    public class UnitsFragment  : Fragment
    {
        public VideosViewModel Vm => App.Locator.Videos;

        public UserViewModel UserVm => App.Locator.User;

  
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.GenericExpandableList, container, false);


            var list = view.FindViewById<ExpandableListView>(Resource.Id.expandableList);
            var units = UserVm.Model.GroupedUnits;
            
            list.SetAdapter(new ExpandableUnitAdapter(Context, units));

            return view;
        }
    }
}