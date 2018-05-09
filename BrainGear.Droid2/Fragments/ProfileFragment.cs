using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using BrainGear.Data;
using BrainGear.UI.Droid.Adaptors;
using System.Threading.Tasks;
using Android.Support.V4.View;
using Android.Graphics;
using System.IO;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using BrainGear.Data.DataServices;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using BrainGear.UI.Droid.Helpers;
using GalaSoft.MvvmLight.Helpers;


namespace BrainGear.UI.Droid
{
    public class ProfileFragment : Fragment
    {
        private const int SelectProfilePic = 1;
        private Binding<string, string> _courseBinding;
        private TextView _courseText;
        private ImageView _imageView;
        private RecyclerView _recyclerView;
        private Binding<string, string> _nameBinding;
        private TextView _nameText;
        private Binding<string, string> _profilePicBinding;
        private string _profilePicUrl;
        private Binding<string, string> _scoreBinding;
        private TextView _scoreText;
        private ImageButton _uploadButton;
        private FloatingActionButton _editButton;
        private ViewPager _viewPager;

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

        public UserViewModel vm => App.Locator.User;

        public TextView NameTextView => _nameText;

        public RecyclerView RecyclerView => _recyclerView;

        public TextView ScoreTextView => _scoreText;

        public TextView CourseTextView => _courseText;

        private void RefreshImageView()
        {
            Square.Picasso.Picasso.With(Activity).Load(ProfilePicUrl).Fit().CenterCrop().Into(_imageView);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.PageProfile, container, false);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            //Add header to list view.
            var headerView = inflater.Inflate(Resource.Layout.ItemProfileHeader, RecyclerView, false);
            _nameText = headerView.FindViewById<TextView>(Resource.Id.textName);
            _courseText = headerView.FindViewById<TextView>(Resource.Id.textCourse);
            _scoreText = headerView.FindViewById<TextView>(Resource.Id.textScore);
            _imageView = headerView.FindViewById<ImageView>(Resource.Id.imageView);
            _uploadButton = headerView.FindViewById<ImageButton>(Resource.Id.button1);
            _editButton = headerView.FindViewById<FloatingActionButton>(Resource.Id.fab);

            _editButton.Click += _editButton_Click;

            //Binding
            _nameBinding = this.SetBinding(() => vm.Model.Name, () => NameTextView.Text);
            _courseBinding = this.SetBinding(() => vm.Model.Course, () => CourseTextView.Text);
            _scoreBinding = this.SetBinding(() => vm.ScoreFormatted, () => ScoreTextView.Text);
            _profilePicBinding = this.SetBinding(() => vm.Model.ProfilePicUrl, () => ProfilePicUrl);
            _uploadButton.Click += UploadButtonOnClick;

            return view;
        }

        private void _editButton_Click(object sender, EventArgs e)
        {
            // Enable controls for editing
            _uploadButton.Visibility = ViewStates.Visible;
            _nameText.InputType = InputTypes.TextVariationNormal;
        }

        public override void OnResume()
        {
            vm.RefreshCommand.Execute(null);
            base.OnResume();
        }

        private void UploadButtonOnClick(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Profile Picture"), SelectProfilePic);
        }

        private View GetView(int position, Achievement item, View convertView)
        {
            var view = convertView ?? Activity.LayoutInflater.Inflate(Resource.Layout.ItemAchievement, null); // re-use an existing view, if one is available
            view.FindViewById<TextView>(Resource.Id.text1).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.text2).Text = item.Description;
            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            progressBar.Max = item.Goal;
            progressBar.Progress = item.Progress;
            view.FindViewById<CheckBox>(Resource.Id.checkBox).Checked = item.Complete;
            return view;
        }

        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == SelectProfilePic) && (resultCode == Result.Ok) && (data != null))
            {
                var uri = data.Data;
                var stream = Activity.ContentResolver.OpenInputStream(uri);
                var bitmap = BitmapFactory.DecodeStream(stream);

                _imageView.SetImageBitmap(bitmap);
                var result = CompressPic(bitmap);
                Toast.MakeText(Activity, "Starting Upload", ToastLength.Short).Show();
                vm.UploadPictureCommand.Execute(result);
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
}