using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BrainGear.Data.DataServices;

namespace BrainGear.Data.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class VideosViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private RelayCommand _refreshCommand;
        private RelayCommand<VideoViewModel> _showCommand;
        private bool _isRefreshing;

        public ObservableCollection<VideoViewModel> Videos
        {
            get;
            private set;
        }

        public VideosViewModel(INavigationService nav, IDataService data)
        {
            _navigationService = nav;
            _dataService = data;
            Videos = new ObservableCollection<VideoViewModel>();
        }

        public RelayCommand<VideoViewModel> ShowCommand => _showCommand ?? (_showCommand = new RelayCommand<VideoViewModel>(ShowVideo, video => video != null));

        public RelayCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(RefreshVideos));

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(() => IsRefreshing, ref _isRefreshing, value); }
        }

        private async void RefreshVideos()
        {
            try
            {
                IsRefreshing = true;
                Videos.Clear();
                var result = await _dataService.GetVideos();
                foreach (var item in result.Select(v => new VideoViewModel(_dataService, v)))
                {
                    Videos.Add(item);
                }
            }
            catch (Exception e)
            {
                var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                dialog.ShowError(e, "Error Refreshing", "Ok", null);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void ShowVideo(VideoViewModel video)
        {
            if (!ShowCommand.CanExecute(video))
            {
                return;
            }

            _navigationService.NavigateTo(ViewModelLocator.VIDEO_DETAILS, video);
        }
    }

}