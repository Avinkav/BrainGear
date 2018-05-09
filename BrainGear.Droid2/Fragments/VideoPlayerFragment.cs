using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using BrainGear.Data;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Adapters;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace BrainGear.UI.Droid.Fragments
{
    public class VideoPlayerFragment : Fragment
    {
        Bundle _webViewState;
        Video _video;
        CommentAdapter _commentAdapter;

        private ListView _listView;
        private WebView _webView;
        private TextView _textDesc;
        private TextView _textVotes;
        private EditText _editComment;
        private ToggleButton _voteButton;
        private Button _commentButton;
        private SwipeRefreshLayout _swipeContainer;
        private Binding<string, string> _votesBinding;
        private Binding<bool, bool> _voteStatusBinding;
        private Binding<string, string> _commentBinding;

        public VideoViewModel Vm { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.PageVideoPlayer, container, false);

            //Find all controls that need programmatic access
            _webView = view.FindViewById<WebView>(Resource.Id.webView1);
            _listView = view.FindViewById<ListView>(Resource.Id.listComments);
            _textDesc = view.FindViewById<TextView>(Resource.Id.textDesc);
            _textVotes = view.FindViewById<TextView>(Resource.Id.textVotes);
            _editComment = view.FindViewById<EditText>(Resource.Id.editComment);
            _commentButton = view.FindViewById<Button>(Resource.Id.buttonComment);
            _voteButton = view.FindViewById<ToggleButton>(Resource.Id.buttonVote);

            _webView.Settings.JavaScriptEnabled = true;

            Vm = Navigator.GetAndRemoveParameter<VideoViewModel>(ViewModelLocator.VIDEO_DETAILS);

            //Static data
            _webView.LoadUrl(Vm.FormattedURL);
            _textDesc.Text = Vm.Model.Description;


            //Bind data
            _votesBinding = this.SetBinding(() => Vm.VotesFormatted, () => TextVotes.Text);
            _voteStatusBinding = this.SetBinding(() => Vm.Model.HasUserVoted, () => VoteButton.Checked,
                BindingMode.OneWay);
            _commentBinding = this.SetBinding(() => EditComment.Text);
            VoteButton.SetCommand("Click", Vm.VoteCommand);
            CommentButton.SetCommand("Click", Vm.SaveCommentCommand, _commentBinding);

            ListView.Adapter = Vm.Model.Comments.GetAdapter(GetView);
            return view;
        }

        public View GetView(int position, Comment comment, View convertView)
        {
            var item = comment;
            var view = convertView ?? Activity.LayoutInflater.Inflate(Resource.Layout.ItemComment, ListView, false); // re-use an existing view, if one is available
            view.FindViewById<TextView>(Resource.Id.text1).Text = item.Text;
            view.FindViewById<TextView>(Resource.Id.text2).Text = item.UserName;
            return view;
        }

        #region Properties

        private MyNavigationService Navigator => (MyNavigationService)ServiceLocator.Current.GetInstance<INavigationService>();

        public SwipeRefreshLayout SwipeContainer => _swipeContainer;

        public ListView ListView => _listView;

        public TextView TextDesc => _textDesc;

        public TextView TextVotes => _textVotes;

        public EditText EditComment => _editComment;

        public ToggleButton VoteButton => _voteButton;

        public Button CommentButton => _commentButton;

        #endregion


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
        }


    }
}