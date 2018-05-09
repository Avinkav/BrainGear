using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.ViewHolders;
using Android.Support.V4.App;
using BrainGear.UI.Droid.Helpers;
using Fragment = Android.Support.V4.App.Fragment;

namespace BrainGear.UI.Droid.Fragments
{
    public class RelatedVideosFragment : Fragment
    {
        private RecyclerView _recylerView;
        public VideosViewModel Vm => App.Locator.Videos;

        public RecyclerView RecylerView => _recylerView;
    


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.GenericRecycler, container, false);
            _recylerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            RecylerView.SetLayoutManager(new LinearLayoutManager(Context));
            RecylerView.SetAdapter(Vm.Videos.GetRecyclerAdapter(OnBindFunc, OnCreateFunc));
            return view;
        }

        public override void OnStart()
        {
            Vm.RefreshCommand.Execute(null);
            base.OnStart();
        }

        private RecyclerView.ViewHolder OnCreateFunc(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemVideo, parent, false);
            return new VideoViewHolder(view, OnItemClick);
        }

        private void OnBindFunc(RecyclerView.ViewHolder viewHolder, int position, VideoViewModel videoVm)
        {
            var vh = viewHolder as VideoViewHolder;
            vh.TextTitle.Text = videoVm.Model.Title;
            vh.TextDescription.Text = videoVm.Model.Description;
            Square.Picasso.Picasso.With(Context).Load(videoVm.Model.ThumbnailUrl).Fit().CenterInside().Placeholder(Android.Resource.Drawable.IcDialogInfo)
               .Into(vh.ImageView);
        }

        public void OnItemClick(object sender, int position)
        {
            Vm.ShowCommand.Execute(Vm.Videos[position]);
        }
    }
}