using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;

namespace BrainGear.Data.Model
{
    public class Wiki : ObservableObject, IFeedItem
    {
        private string _title;
        private string _summary;
        public string Id { get; set; }

        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }

        public string Summary
        {
            get { return _summary; }
            set { Set(() => Summary, ref _summary, value); }
        }

        [IgnoreDataMember]
        public ObservableCollection<WikiSection> Sections { get; set; }

        public int Revision { get; set; }

        public string EditorId { get; set; }

        public string ThumbnailUrl { get; set; }

        public string RelatedUnits { get; set; }

        [IgnoreDataMember]
        public Type Type => typeof (Wiki);

        public Wiki()
        {
            Sections = new ObservableCollection<WikiSection>();
        }
    }
}
