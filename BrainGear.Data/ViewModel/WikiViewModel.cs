using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainGear.Data.DataServices;
using BrainGear.Data.Model;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace BrainGear.Data.ViewModel
{
    public class WikiViewModel : ViewModelBase, IFeedItemViewModel
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private ObservableCollection<WikiSection> _list;
        private Wiki _model;
        private RelayCommand _refreshCommand;
        private RelayCommand _saveCommand;


        public WikiViewModel(IDataService dataService, INavigationService navigationService, Wiki item)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _model = item;
        }

        private UserViewModel _userViewModel => ServiceLocator.Current.GetInstance<UserViewModel>();
        public Wiki Model => _model;

        public List<string> Segments => _model.Sections.Select(s => s.Title).ToList();

        public RelayCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(Refresh));

        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(Save));

        public ObservableCollection<WikiSection> WikiAsCollection => _list;
        public Type Type => typeof(WikiViewModel);
        public string RelatedTo { get; set; }

        private void Save()
        {
            // Create new revision
            Model.Revision++;
            Model.EditorId = _userViewModel.Model.Id;
        }

        private ObservableCollection<WikiSection> ToCollection()
        {
            var list = new ObservableCollection<WikiSection>
            {
                new WikiSection
                {
                    Id =  Model.Id,
                    Title = Model.Title,
                    Content = Model.Summary,
                    Position = -1
                }
            };
            foreach (var wikiSection in _model.Sections)
            {
                list.Add(wikiSection);
            }
            return list;
        }

        private async void Refresh()
        {
            _model = await _dataService.GetFullWiki(_model.Id);
            _list = ToCollection();
        }
    }
}
