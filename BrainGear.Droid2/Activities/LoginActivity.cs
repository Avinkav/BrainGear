using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using GalaSoft.MvvmLight.Helpers;
using BrainGear.Data.ViewModel;

namespace BrainGear.UI.Droid
{
    [Activity(Theme = "@style/PrimaryTheme",  Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        private EditText _passwordText;
        private EditText _userIdText;
        private Binding<string, string> _userIdBinding;
        private Binding<string, string> _passwordBinding;
        private Binding<bool, bool> _loggedInBinding;
        private bool _isLoggedIn;
        public bool IsLoggedIn {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                if (value == true)
                    CompleteLogin();
            }
        }

        private void CompleteLogin()
        {
            PreferenceManager.GetDefaultSharedPreferences(this).Edit().PutString("UserId", Vm.LoginId).Commit();
            SetResult(Result.Ok);
            Finish();
        }

        public LoginViewModel Vm => App.Locator.Login;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            SetContentView(Resource.Layout.PageLogin);

            var button = FindViewById<Button>(Resource.Id.MyButton);
            _userIdText = FindViewById<EditText>(Resource.Id.userid);
            _passwordText = FindViewById<EditText>(Resource.Id.password);

            _userIdBinding = this.SetBinding(() => UserIdText.Text, () => Vm.LoginId);
            _passwordBinding = this.SetBinding(() => PasswordText.Text, () => Vm.Password);
            _loggedInBinding = this.SetBinding(() => Vm.IsLoggedIn, () => IsLoggedIn);

            button.SetCommand("Click", Vm.LoginCommand);
                

        }

        public EditText PasswordText => _passwordText;

        public EditText UserIdText => _userIdText;

        public override void OnBackPressed()
        {
            
        }
    }
}

