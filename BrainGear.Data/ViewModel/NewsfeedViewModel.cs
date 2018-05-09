using System;
using System.Collections.ObjectModel;
using BrainGear.Data.DataServices;
using BrainGear.Data.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace BrainGear.Data.ViewModel
{
    public class NewsfeedViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private bool _isRefreshing;
        private RelayCommand _refreshCommand;
        private RelayCommand<IFeedItemViewModel> _showCommand;

        public NewsfeedViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            Model = new ObservableCollection<IFeedItemViewModel>();
        }

        public ObservableCollection<IFeedItemViewModel> Model { get; private set; }

        public RelayCommand<IFeedItemViewModel> ShowCommand
            => _showCommand ?? (_showCommand = new RelayCommand<IFeedItemViewModel>(ShowFeedItem));

        public RelayCommand RefreshCommand 
            => _refreshCommand ?? (_refreshCommand = new RelayCommand(RefreshFeed));

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(() => IsRefreshing, ref _isRefreshing, value); }
        }

        private async void RefreshFeed()
        {
            try
            {
                IsRefreshing = true;
                var result = await _dataService.GetNewsfeed();
                Model.Clear();
                foreach (var item in result)
                {
                    IFeedItemViewModel vm;
                    if (item.Type == typeof (Video))
                    {
                        vm = new VideoViewModel(_dataService, (Video) item);
                        Model.Add(vm);
                    }
                    else if (item.Type == typeof (Wiki))
                    {
                        vm = new WikiViewModel(_dataService, _navigationService, (Wiki) item);
                        Model.Add(vm);
                    }
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

        private void ShowFeedItem(IFeedItemViewModel feedItem)
        {
            if (feedItem == null) return;

            if (feedItem.Type == typeof (VideoViewModel))
            {
                _navigationService.NavigateTo(ViewModelLocator.VIDEO_DETAILS, (VideoViewModel)feedItem);
            }else if (feedItem.Type == typeof(WikiViewModel))
            {
                _navigationService.NavigateTo(ViewModelLocator.WIKI_DETAILS, (WikiViewModel)feedItem);
            }
        }
    }
}