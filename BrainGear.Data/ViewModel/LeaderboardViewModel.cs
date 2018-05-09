using System.Collections.ObjectModel;
using BrainGear.Data.DataServices;
using BrainGear.Data.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace BrainGear.Data.ViewModel
{
    public class LeaderboardViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private RelayCommand _refreshCommand;

        public ObservableCollection<Leaderboard> Model { get; private set; }

        public RelayCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(Refresh));

        private async void Refresh()
        {
            var result = await _dataService.GetLeaderboard();
            Model.Clear();
            foreach (var item in result)
            {
                Model.Add(item);
            }
        }

        public LeaderboardViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            Model = new ObservableCollection<Leaderboard>();
        }
    }
}