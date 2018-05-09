using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;

namespace BrainGear.Data.Model
{
    public class Video : ObservableObject, IFeedItem
    {
        private bool _hasUserVoted;
        private int _votes;
        private string _title;

        public Video()
        {
            Comments = new ObservableCollection<Comment>();
        }

        public string Id { get; set; }

        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }

        public string Description { get; set; }

        public string RelatedUnits { get; set; }

        public string Tags { get; set; }

        public string EmbedUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        [IgnoreDataMember]
        public Type Type => typeof (Video);

        public int Votes
        {
            get { return _votes; }
            set { Set(() => Votes, ref _votes, value); }
        }

        [IgnoreDataMember]
        public bool HasUserVoted
        {
            get { return _hasUserVoted; }
            set { Set(() => HasUserVoted, ref _hasUserVoted, value); }
        }

        [IgnoreDataMember]
        public ObservableCollection<Comment> Comments { get; set; }
    }
}

