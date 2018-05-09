using GalaSoft.MvvmLight;

namespace BrainGear.Data.Model
{
    public class WikiSection : ObservableObject
    {
        private string _title;
        private string _content;
        public string Id { get; set; }
        public string WikiId { get; set; }
        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }

        public int Position { get; set; }

        public string Content
        {
            get { return _content; }
            set { Set(() => Content, ref _content, value); }
        }
    }
}