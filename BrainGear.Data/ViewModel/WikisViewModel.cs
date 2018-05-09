using System.Collections.ObjectModel;
using BrainGear.Data.DataServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace BrainGear.Data.ViewModel
{
    public class WikisViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private INavigationService _navigationService;
        private RelayCommand _refreshCommand;
        private RelayCommand<WikiViewModel> _showCommand;

        public ObservableCollection<WikiViewModel> Model { get; private set; }

        public RelayCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(Refresh));

        public RelayCommand<WikiViewModel> ShowCommand
            => _showCommand ?? (_showCommand = new RelayCommand<WikiViewModel>(ShowDetails, w => w != null));

        private void ShowDetails(WikiViewModel wikiViewModel)
        {
            wikiViewModel.RefreshCommand.Execute(null);
            _navigationService.NavigateTo(ViewModelLocator.WIKI_DETAILS, wikiViewModel);
        }

        private  async void Refresh()
        {
            var result = await _dataService.GetWikis();
            Model.Clear();
            foreach (var item in result)
            {
                Model.Add(new WikiViewModel(_dataService, _navigationService, item));
            }
        }

        [PreferredConstructor]
        public WikisViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            Model = new ObservableCollection<WikiViewModel>();
        }
    }
}