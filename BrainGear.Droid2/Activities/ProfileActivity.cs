using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Helpers;
using GalaSoft.MvvmLight.Helpers;

namespace BrainGear.UI.Droid.Activities
{
    [Activity(Label = "User Profile", Theme = "@style/TransparentStatusBarTheme")]
    public class ProfileActivity : AppCompatActivity
    {
        private const int SelectProfilePic = 1;
        private Binding<string, string> _courseBinding;
        private FloatingActionButton _editFloatingButton;
        private ImageView _imageView;
        private Binding<string, string> _nameBinding;
        private Binding<string, string> _profilePicBinding;
        private string _profilePicUrl;
        private Binding<string, string> _scoreBinding;
        private ImageButton _uploadButton;
        private RecyclerView _recyclerView;

        public RecyclerView RecyclerView => _recyclerView;

        public string ProfilePicUrl
        {
            get { return _profilePicUrl; }
            set
            {
                if (_profilePicUrl != value)
                {
                    _profilePicUrl = value;
                    RefreshImageView();
                }
            }
        }

        public UserViewModel Vm => App.Locator.User;

        private void RefreshImageView()
        {
            Square.Picasso.Picasso.With(this).Load(ProfilePicUrl).Into(_imageView);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            SetContentView(Resource.Layout.PageProfile);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView);
            _editFloatingButton = FindViewById<FloatingActionButton>(Resource.Id.fab);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            //_uploadButton = FindViewById<ImageButton>(Resource.Id.button1);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.profile_title);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            _editFloatingButton.Click += _editButton_Click;

            //Binding
            _nameBinding = this.SetBinding(() => Vm.Model.Name, () => SupportActionBar.Title);
            _profilePicBinding = this.SetBinding(() => Vm.Model.ProfilePicUrl, () => ProfilePicUrl);
            RecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            RecyclerView.SetAdapter(new ProfileAdapter<Achievement>(Vm, Vm.Model.Achievements));
            //_uploadButton.Click += UploadButtonOnClick;

            //Hide as not implemented
            _editFloatingButton.Hide();
        }

        private void _editButton_Click(object sender, EventArgs e)
        {
            // Enable controls for editing
            //_uploadButton.Visibility = ViewStates.Visible;
            //_nameText.InputType = InputTypes.TextVariationNormal;
        }

        protected override void OnResume()
        {
            Vm.RefreshCommand.Execute(null);
            base.OnResume();
        }

        private void UploadButtonOnClick(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Profile Picture"), SelectProfilePic);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == SelectProfilePic) && (resultCode == Result.Ok) && (data != null))
            {
                var uri = data.Data;
                var stream = ContentResolver.OpenInputStream(uri);
                var bitmap = BitmapFactory.DecodeStream(stream);

                _imageView.SetImageBitmap(bitmap);
                var result = CompressPic(bitmap);
                Toast.MakeText(this, "Starting Upload", ToastLength.Short).Show();
                Vm.UploadPictureCommand.Execute(result);
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        private byte[] CompressPic(Bitmap bitmap)
        {
            var outStream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);
            return outStream.ToArray();
        }
    }

    class ProfileAdapter<T> : ObservableRecylcerAdapter<T>
    {
        private const int AchievementType = 1;
        private const int UserType = 0;
        private UserViewModel _user;

        public override int ItemCount => 1 + (_list?.Count ?? 0);

        public override int GetItemViewType(int position) => (position == 0) ? UserType : AchievementType;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == AchievementType)
            {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemAchievement, parent, false);
                return new AchievementViewHolder(view);
            }
            else
            {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemProfileHeader, parent, false);
                return new ProfileViewHolder(view);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position == 0)
            {
                var vh = holder as ProfileViewHolder;
                vh.QutIdText.Text = _user.Model.QutId;
                vh.EmailText.Text = _user.Model.Email;
                vh.CourseText.Text = _user.Model.Course;
                vh.ScoreText.Text = _user.ScoreFormatted;
            }
            else
            {
                position--;
                var vh = holder as AchievementViewHolder;
                vh.TitleText.Text = _user.Model.Achievements[position].Title;
                vh.DescriptionText.Text = _user.Model.Achievements[position].Description;
                vh.ProgressBar.Max = _user.Model.Achievements[position].Goal;
                vh.ProgressBar.Progress = _user.Model.Achievements[position].Progress;
                vh.CompletedCheckBox.Checked = _user.Model.Achievements[position].Complete;
            }
        }

        public ProfileAdapter(UserViewModel user, IList<T> data)
        {
            _user = user;
            DataSource = data;
        }
    }

    internal class ProfileViewHolder : RecyclerView.ViewHolder
    {
        public ProfileViewHolder(View view) : base(view)
        {
            QutIdText = view.FindViewById<TextView>(Resource.Id.textQutId);
            EmailText = view.FindViewById<TextView>(Resource.Id.textEmail);
            CourseText = view.FindViewById<TextView>(Resource.Id.textCourse);
            ScoreText = view.FindViewById<TextView>(Resource.Id.textScore);
        }

        public TextView QutIdText { get; private set; }
        public TextView EmailText { get; private set; }
        public TextView CourseText { get; private set; }
        public TextView ScoreText { get; private set; }
    }

    internal class AchievementViewHolder : RecyclerView.ViewHolder
    {
        public AchievementViewHolder(View view) : base(view)
        {
            TitleText = view.FindViewById<TextView>(Resource.Id.text1);
            DescriptionText = view.FindViewById<TextView>(Resource.Id.text2);
            ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            CompletedCheckBox = view.FindViewById<CheckBox>(Resource.Id.checkBox);
        }

        public CheckBox CompletedCheckBox { get; private set; }
        public TextView DescriptionText { get; private set; }
        public ProgressBar ProgressBar { get; private set; }
        public TextView TitleText { get; private set; }
    }
}