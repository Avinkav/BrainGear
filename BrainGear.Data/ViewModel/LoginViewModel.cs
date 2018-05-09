using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainGear.Data.DataServices;
using BrainGear.Data.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace BrainGear.Data.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private INavigationService _navigationService;
        private string _loginId;
        private string _password;
        private RelayCommand _loginCommand;
        private bool _isLoggedIn;

        public string LoginId
        {
            get { return _loginId; }
            set { Set(() => LoginId, ref _loginId, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(() => Password, ref _password, value); }
        }

        public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(Login));

        public async void Login()
        {
            if (IsLoggedIn) return;
            var user = await _dataService.Login(LoginId, Password);
            if (user != null)
            {
                var userVm = new UserViewModel(_navigationService, _dataService, user);
                SimpleIoc.Default.Register(() => userVm);
                IsLoggedIn = true;
            }
            else
            {
                var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                dialog.ShowMessage("Invalid Id. Try n9055509. Password can be blank for now", "Login Failed", "Retry", null);
            }
        }

        // Flag indicates to UI that login has succceeded.
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Set(() => IsLoggedIn, ref _isLoggedIn, value); }
        }

        public LoginViewModel(IDataService dataService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }


        private User CreateTestUser()
        {
            return new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Avin Abeyratne",
                Course = "Bachelor of Information Technology (Computer Science)",
                Email = "avinkavesha@gmail.com",
                Score = 100,
                QutId = "n9055509",
                Units = new ObservableCollection<Unit>
                {
                    new Unit
                    {
                        Code = "CAB240",
                        Name = "Information Security",
                        Semester = "2015/02"
                    },
                    new Unit
                    {
                        Code = "CAB303",
                        Name = "Networks",
                        Semester = "2015/02"
                    },new Unit
                    {
                        Code = "IAB330",
                        Name = "Mobile Application Development",
                        Semester = "2015/02"
                    },new Unit
                    {
                        Code = "IFB299",
                        Name = "Application Design and Development",
                        Semester = "2015/02"
                    }
                },
                ProfilePicUrl = "https://braingear.blob.core.windows.net/profile/n9055509.jpeg"
            };
        }


        public async Task<UserViewModel> LoginAndGetUser()
        {
            if (IsLoggedIn) return null;
            var user = await _dataService.Login(LoginId, Password);
            if (user != null)
            {
                IsLoggedIn = true;
                var userVm = new UserViewModel(_navigationService, _dataService, user);
                return userVm;
            }
            else
            {
                var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                dialog.ShowMessage("Invalid saved Login", "Login Failed", "Retry", null);
                return null;
            }
        }
    }
}
