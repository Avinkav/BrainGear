using System;

namespace BrainGear.Data.Model
{
    public class Comment 
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string VideoId { get; set; }
    }
}
