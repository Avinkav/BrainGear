using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using BrainGear.Data.DataServices;
using BrainGear.Data.Model;

namespace BrainGear.Data.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private IDialogService _dialogService => ServiceLocator.Current.GetInstance<IDialogService>();
        private RelayCommand<byte[]> _uploadPictureCommand;
        private RelayCommand _saveCommand;
        private RelayCommand _refreshCommand;
        private User _model;
        public string UserId;
        public User Model
        {
            get
            {
                if (_model != null)
                    return _model;

                throw new Exception("User being accessed when not present. Logic plz");
            }
            private set { _model = value; }
        }

        public RelayCommand<byte[]> UploadPictureCommand
            =>_uploadPictureCommand ?? (_uploadPictureCommand = new RelayCommand<byte[]>(UploadPicture, stream => stream != null));
        public List<string> UnitsList => _model.Units.Select(u => u.Code).ToList();
        public RelayCommand SaveCommand 
            => _saveCommand ?? (_saveCommand = new RelayCommand(SaveUser));

        public RelayCommand RefreshCommand 
            => _refreshCommand ?? (_refreshCommand = new RelayCommand(Refresh));


        public string ScoreFormatted => Model.Score.ToString();

        private async void Refresh()
        {
            if (UserId != "")
            {

                _model = await _dataService.GetUser(UserId);
                RaisePropertyChanged();
            }

            else
            {    
                _dialogService.ShowError("User Id not set by view", "Error", "Ok", null);
            }

        }

        private void SaveUser()
        {
            _dataService.SaveUser(_model);
        }

        private void UploadPicture(byte[] bytes)
        {
            _dataService.UploadPicture(bytes);
        }

        public UserViewModel(INavigationService navigationService, IDataService dataService, User user)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            Model = user;
        }
    }
}
