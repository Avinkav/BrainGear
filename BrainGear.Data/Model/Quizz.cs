using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace BrainGear.Data.Model
{
    public class Quizz : ObservableObject, IFeedItem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }
        public string RelatedUnits { get; set; }

        public Type Type => typeof (Quizz);
    }
}
