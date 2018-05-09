using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Helpers;
using BrainGear.UI.Droid.ViewHolders;

namespace BrainGear.UI.Droid.Fragments
{
    public class WikiReadFragment : Fragment
    {
        private RecyclerView _recyclerView;
        public WikiViewModel Vm { get; set; }
        public RecyclerView RecyclerView => _recyclerView;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.GenericRecycler, container, false);

            var adapter = Vm.WikiAsCollection.GetRecyclerAdapter(OnBindFunc, OnCreateFunc);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            RecyclerView.SetAdapter(adapter);

            return view;
        }


        private RecyclerView.ViewHolder OnCreateFunc(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemWikiDetail, parent, false);
            return new WikiDetailViewHolder(view);
        }

        private void OnBindFunc(RecyclerView.ViewHolder viewHolder, int position, WikiSection item)
        {
            var vh = viewHolder as WikiDetailViewHolder;
            vh.TitleText.Text = item.Title;
            vh.ContentText.Text = item.Content;
        }
    }
}
