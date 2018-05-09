using System;

namespace BrainGear.Data.ViewModel
{
    public interface IFeedItemViewModel
    {
        string RelatedTo { get; set; }

        Type Type { get; }
    }
}