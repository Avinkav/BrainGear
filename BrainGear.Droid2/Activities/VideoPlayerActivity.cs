using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Webkit;
using Android.Widget;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Adapters;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace BrainGear.UI.Droid.Activities
{
    [Activity(Label = "VideoDetailsActivity")]
    public class VideoPlayerActivity : AppCompatActivity
    {
        private ListView _listView;
        private WebView _webView;
        private TextView _textDesc;
        private TextView _textVotes;
        private EditText _editComment;
        private ToggleButton _voteButton;
        private Button _commentButton;
        private Binding<string, string> _votesBinding;
        private Binding<bool, bool> _voteStatusBinding;
        private Binding<string, string> _commentBinding;



        private MyNavigationService Navigator => (MyNavigationService)ServiceLocator.Current.GetInstance<INavigationService>();

        public ListView ListView => _listView;

        public TextView TextDesc => _textDesc;

        public TextView TextVotes => _textVotes;

        public EditText EditComment => _editComment;

        public ToggleButton VoteButton => _voteButton;

        public Button CommentButton => _commentButton;

        public VideoViewModel Vm { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            SetContentView(Resource.Layout.PageVideoPlayer);

            _webView = FindViewById<WebView>(Resource.Id.webView1);
            _listView = FindViewById<ListView>(Resource.Id.listComments);
            _editComment = FindViewById<EditText>(Resource.Id.editComment);
            _commentButton = FindViewById<Button>(Resource.Id.buttonComment);

            var view = LayoutInflater.Inflate(Resource.Layout.HeaderVideoPlayer, ListView, false);
            _listView.AddHeaderView(view);
            _textDesc = view.FindViewById<TextView>(Resource.Id.textDesc);
            _textVotes = view.FindViewById<TextView>(Resource.Id.textVotes);
            _voteButton = view.FindViewById<ToggleButton>(Resource.Id.buttonVote);
            _webView.Settings.JavaScriptEnabled = true;

            try
            {
                var key = Intent.GetStringExtra(MyNavigationService.ParamKey);
                Vm = Navigator.GetAndRemoveParameter<VideoViewModel>(key);
            }
            catch (Exception e)
            {
                Console.WriteLine("Player opened without a video");
                Finish();
                return;
            }

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
            _commentButton.Click += (sender, args) =>
            {
                ((InputMethodManager) GetSystemService(InputMethodService)).HideSoftInputFromWindow(
                    CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                _editComment.Text = "";
            };
            _editComment.Click += delegate { _listView.SetSelection(Vm.Model.Comments.Count - 1); };

            ListView.Adapter = Vm.Model.Comments.GetAdapter(GetView);
        }

        public View GetView(int position, Comment comment, View convertView)
        {
            var item = comment;
            var view = convertView ?? LayoutInflater.Inflate(Resource.Layout.ItemComment, ListView, false); // re-use an existing view, if one is available
            view.FindViewById<TextView>(Resource.Id.text1).Text = item.Text;
            view.FindViewById<TextView>(Resource.Id.text2).Text = item.UserName;
            return view;
        }
    
    }
}