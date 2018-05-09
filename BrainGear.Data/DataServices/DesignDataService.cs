using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using Video = BrainGear.Data.Model.Video;

namespace BrainGear.Data.DataServices
{
    class DesignDataService : IDataService
    {
        private readonly Random _random = new Random();
        private List<Video> _videos;
        private User _user;

        private List<Video> Videos => _videos ?? (_videos = CreateVideos(50));

        public User User => _user ?? (_user = new User()
        {
            Id = Guid.NewGuid().ToString(),
            QutId = "n9055509",
            Email = "avinkavesha@gmail.com",
            Name = "Avin Abeyratne",
            Course = "Bachelor Of IT",
            Score = 100,
            Units = SeedUnits(20),
            Achievements = SeedAchievements(20),
            ProfilePicUrl = "https://braingear.blob.core.windows.net/profile/n9055509.jpeg"
			});

        private ObservableCollection<Unit> SeedUnits(int i)
        {
            var list = new ObservableCollection<Unit>();
            for (int j = 0; j < i; j++)
            {
                list.Add(new Unit
                {
                    Code = "CAB240",
                    Name = "Blurrrgh",
                    Semester = "2015/02"
                });
            }
            return list;
        }

        public Task<List<Video>> GetVideos()
        {

            return Task.FromResult(Videos);
        }

        public Task<bool> SaveVideo(Video video)
        {
            var match = Videos.FindIndex( v => v.Id == video.Id);
            if (match == -1)
                return Task.FromResult(false);

                Videos[match] = video;
                return Task.FromResult(true);
            
        }

        public Task<User> GetUser(string userid)
        {
            var user = new User
            {
                Id = User.Id,
                Name = User.Name,
                Course = User.Course,
                QutId = User.QutId,
                Units = User.Units,
                Score = User.Score,
                ProfilePicUrl = User.ProfilePicUrl
            };

            user.Achievements.Clear();
            foreach (var item in User.Achievements)
            {
                user.Achievements.Add(item);
            }
            return Task.FromResult(user);
        }

        public Task<bool> SaveUser(User user)
        {
            return Task.FromResult(false);
        }

        public Task<List<IFeedItem>> GetNewsfeed()
        {
            var list = new List<IFeedItem>();

            list.AddRange(CreateVideos(10));
            list.AddRange(CreateWikis(5));
            list.Shuffle();

            return Task.FromResult(list);
        }

        public Task<bool> UploadPicture(byte[] bytes)
        {
            return Task.FromResult(true);
        }

        public Task<List<Leaderboard>> GetLeaderboard()
        {
            var list = new List<Leaderboard>();
            for (int i = 0; i < 50; i++)
            {
                list.Add(new Leaderboard
                {
                   UserName = "Test User",
                   Score = _random.Next(1000)
                });
            }
            list.Sort((a,b) => a.Score - b.Score);
            return Task.FromResult(list);
        }

        public Task<List<Wiki>> GetWikis()
        {
            return Task.FromResult(CreateWikis(30));
        }

        public Task<Wiki> GetFullWiki(string id)
        {
            var wiki = CreateWiki();
            for (int i = 0; i < 5; i++)
            {
                wiki.Sections.Add(new WikiSection
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Lorem Ipsum",
                   Content = LoremIpsum(5),
                });
            }
            return Task.FromResult(wiki);

        }

        #region Helpers

        private ObservableCollection<Achievement> SeedAchievements(int count)
        {
            var list = new ObservableCollection<Achievement>();
            for (var i = 0; i < count; i++)
            {
                list.Add(
                    new Achievement()
                    {
                        Title = "Test 1",
                        Description = "Test Desc",
                        Goal = 20,
                        Progress = _random.Next(20)
                    });
            }
            return list;
        }

        private List<Video> CreateVideos(int count)
        {
            var list = new List<Video>();
            for (var i = 0; i < count; i++)
            {
                list.Add(CreateVideo());
            }
            return list;
        }


        private List<Wiki> CreateWikis(int count)
        {
            var list = new List<Wiki>();
            for (var i = 0; i < count; i++)
            {
                list.Add(CreateWiki());
            }
            return list;
        }

        private Wiki CreateWiki()
        {
            return new Wiki()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Lorem ipsum dolor sit amet" ,
                Summary =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                ThumbnailUrl = "https://blog.rosehosting.com/blog/wp-content/uploads/2015/04/mediawiki.png"
            };
        }

        private Video CreateVideo()
        {
            return new Video()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Task-Tech Fit",
                Description =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                EmbedUrl = "https://www.youtube.com/embed/R9UGr5SpzlQ",
                HasUserVoted = false,
                Tags = "Task-Tech Fit,IFB101,IT,Computer Science",
                Votes = _random.Next(3000),
                RelatedUnits = "IFB101",
                Comments = new ObservableCollection<Comment>()
                {
                    new Comment() { Id = Guid.NewGuid().ToString(), UserId = "n4579736", UserName = "Jimmy", Text = "This is one comment.", CreatedAt = DateTime.UtcNow},
                    new Comment() { Id = Guid.NewGuid().ToString(), UserId = "n4579736", UserName = "Jimmy", Text = "This is one comment.", CreatedAt = DateTime.UtcNow},
                    new Comment() { Id = Guid.NewGuid().ToString(), UserId = "n4579736", UserName = "Jimmy", Text = "This is one comment.", CreatedAt = DateTime.UtcNow},
                    new Comment() { Id = Guid.NewGuid().ToString(), UserId = "n9055509", UserName = "Avin", Text = "This is one really long comment. WOW WTF", CreatedAt = DateTime.UtcNow},
                    new Comment() { Id = Guid.NewGuid().ToString(), UserId = "n4573736", UserName = "John", Text = "This is another comment.", CreatedAt = DateTime.UtcNow}
                },
                ThumbnailUrl = "http://img.youtube.com/vi/R9UGr5SpzlQ/hqdefault.jpg"
            };
        }

        private string LoremIpsum(int paras)
        {
            var str =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.\n\n";
            string output = "";
            for (int i = 0; i < paras; i++)
            {
                output = output + str;
            }
            return str;
        }
        #endregion
    }

    public static class MyExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}