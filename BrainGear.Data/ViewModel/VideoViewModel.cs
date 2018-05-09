using System;
using System.Collections.Generic;
using BrainGear.Data.DataServices;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace BrainGear.Data.ViewModel
{
    public class VideoViewModel : ViewModelBase, IFeedItemViewModel
    {
        private readonly IDataService _dataService;
        private UserViewModel _userViewModel => ServiceLocator.Current.GetInstance<UserViewModel>();
        public string RelatedTo { get; set; }
        public Type Type => typeof (VideoViewModel);

        public Video Model => _model;

        private RelayCommand<string> _saveCommentCommand;

        public RelayCommand<string> SaveCommentCommand
            => _saveCommentCommand ?? (_saveCommentCommand = new RelayCommand<string>(SaveComment, text => !string.IsNullOrEmpty(text)));

        private RelayCommand _voteCommand;
        private Video _model;

        public RelayCommand VoteCommand
            => _voteCommand ?? (_voteCommand = new RelayCommand(Vote));

        public string VotesFormatted => (Votes + " Votes");

        public string FormattedURL => Model.EmbedUrl;

        public int Votes
        {
            get
            {
                return _model.Votes; 
            }
            set
            {
                if (_model.Votes != value)
                {
                    _model.Votes = value;
                    RaisePropertyChanged(() => VotesFormatted);
                }       
            }
        }

        private async void  SaveComment(string text)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Text = text,
                UserId = _userViewModel.Model.Id,
                UserName = _userViewModel.Model.Name,
                VideoId = _model.Id
            };
            _model.Comments.Add(comment);
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
            try
            {
                var response = await _dataService.SaveComment(comment);
                dialog.ShowMessage(response.ToString(),"Response");
            }
            catch (Exception e)
            {
                dialog.ShowError(e, "Error", "OK", null);
            }
        }

        private async void Vote()
        {
            _model.HasUserVoted = !_model.HasUserVoted;
            Votes = _model.HasUserVoted ? Votes + 1 : Votes - 1;
            await _dataService.SaveVideo(_model);
        }

        public VideoViewModel(IDataService dataService, Video video)
        {
            _dataService = dataService;
            _model = video;
        }
    }
}