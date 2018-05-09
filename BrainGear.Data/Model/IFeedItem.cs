using System;

namespace BrainGear.Data.Model
{
   
    /// <summary>
    /// Represents a type that can be displayed on the newsfeed
    /// </summary>
    public interface IFeedItem
    {
        string Id { get; set; }

        string Title { get; set; }

        string ThumbnailUrl { get; set; }

        string RelatedUnits { get; set; }
        /// <summary>
        /// The type of object implmenting this IFeedItem. Used by NewsfeedViewModel to render custom views.
        /// </summary>
        Type Type { get; }



    }
}