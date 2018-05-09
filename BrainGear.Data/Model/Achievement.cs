using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BrainGear.Data.Model
{
    public class Achievement
    {
        private int _progress;
        public string Id { get; set;}
        public string Title { get; set; }

        public string Description { get; set; }

        public int Progress
        {
            get
            {
                return _progress;
            }
            set { _progress = (value > _progress) ? _progress : value; } 
        }

        public int Goal { get; set; }

        [IgnoreDataMember]
        public bool Complete => (Progress >= Goal);

        public string UserId { get; set; }

        public static List<Achievement> SeedAchievements()
        {
            return new List<Achievement>()
            {
                new Achievement {Title = "Novice", Description = "Watch 5 bite-sized videos", Goal = 5},
                new Achievement
                {
                    Title = "Explorer",
                    Description = "Visit all pages on the App",
                    Goal = 10
                },
                new Achievement
                {
                    Title = "Beginner",
                    Description = "Watch 10 bite-sized videos",
                    Goal = 10
                },
                new Achievement {Title = "Contributor", Description = "Edit 50 Wikis", Goal = 50},
                new Achievement
                {
                    Title = "Human Encyclopedia",
                    Description = "Read 200 Wikis",
                    Goal = 200
                },
                new Achievement {Title = "Enthusiast", Description = "Comment on 50 videos", Goal = 50},
                new Achievement
                {
                    Title = "Fan Favourite",
                    Description = "Get 1000 likes on Uploaded Videos",
                    Goal = 1000
                },
                new Achievement
                {
                    Title = "Veteran",
                    Description = "Be involved in the community for an year",
                    Goal = 365
                },
                new Achievement
                {
                    Title = "Elite",
                    Description = "Complete all achievements",
                    Goal = 8
                }
            };
        }
    }
}
