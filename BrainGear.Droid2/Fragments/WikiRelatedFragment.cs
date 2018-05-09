using System;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Helpers;
using BrainGear.UI.Droid.ViewHolders;
using Fragment = Android.Support.V4.App.Fragment;

namespace BrainGear.UI.Droid.Fragments
{
    public class WikiRelatedFragment : Fragment
    {
        public WikisViewModel Vm => App.Locator.Wiki;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.GenericRecycler, container, false);

            var _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _recyclerView.SetLayoutManager(new GridLayoutManager(Activity, 2, 1, false));
            _recyclerView.SetAdapter(Vm.Model.GetRecyclerAdapter(OnViewBinding, OnViewCreate));

            return view;
        }

        public override void OnStart()
        {
            Vm.RefreshCommand.Execute(null);
            base.OnStart();
        }

        private void OnViewBinding(RecyclerView.ViewHolder viewHolder, int position, WikiViewModel wikiViewModel)
        {
            var vh = viewHolder as WikiViewHolder;
            vh.TextTitle.Text = wikiViewModel.Model.Title;
            vh.TextContent.Text = wikiViewModel.Model.Summary;
        }

        private RecyclerView.ViewHolder OnViewCreate(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemWiki, parent, false);
            return  new WikiViewHolder(view, OnClick);
        }

        private void OnClick(Object sender, int position)
        {
            Vm.ShowCommand.Execute(Vm.Model[position]);
        }

        public override string ToString() => "Related Wikis";
    }
}