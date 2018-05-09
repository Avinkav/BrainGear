using System.Runtime.InteropServices.WindowsRuntime;
using BrainGear.Data.DataServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

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
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        private RelayCommand _showNewsfeedCommand;
        private RelayCommand _showVideosCommand;
        private RelayCommand _showWikiCommand;
        private RelayCommand _showProfileCommand;

        //private VideosViewModel _videosViewModel;
        //private ProfileViewModel _profileViewModel;
        //private WikiViewModel _wikiViewModel;
        //private NewsfeedViewModel _newsfeedViewModel;
        //public NewsfeedViewModel NewsfeedViewModel
        //    => _newsfeedViewModel ?? (_newsfeedViewModel = new NewsfeedViewModel(_navigationService, _dataService));

        //public VideosViewModel VideosViewModel
        //    => _videosViewModel ?? (_videosViewModel = new VideosViewModel(_navigationService, _dataService));

        //public ProfileViewModel ProfileViewModel 
        //    => _profileViewModel ?? (_profileViewModel = new ProfileViewModel(_navigationService, _dataService));

        //public WikiViewModel WikiViewModel 
        //    => _wikiViewModel ?? (_wikiViewModel = new WikiViewModel());

        public RelayCommand ShowNewsfeedCommand => _showNewsfeedCommand ?? (_showNewsfeedCommand = new RelayCommand(() => _navigationService.NavigateTo(ViewModelLocator.NEWSFEED) ));

        public RelayCommand ShowVideosCommand => _showVideosCommand ?? (_showVideosCommand = new RelayCommand(() => _navigationService.NavigateTo(ViewModelLocator.VIDEOS)));

        public RelayCommand ShowWikiCommand => _showWikiCommand ?? (_showWikiCommand = new RelayCommand(() => _navigationService.NavigateTo(ViewModelLocator.WIKI)));

        public RelayCommand ShowProfileCommand => _showProfileCommand ?? (_showProfileCommand = new RelayCommand(() => _navigationService.NavigateTo(ViewModelLocator.PROFILE)));

        
        public MainViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

    }
}