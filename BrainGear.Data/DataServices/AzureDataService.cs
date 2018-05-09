using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using Microsoft.Practices.ServiceLocation;


namespace BrainGear.Data.DataServices
{
    public class AzureDataService : IDataService { 

        public static MobileServiceClient Client = new MobileServiceClient(
     "https://braingear-js.azure-mobile.net/",
     "OloaIddBmoSFJGoEGDCiluCvGHmFnV43");

        private User _user => ServiceLocator.Current.GetInstance<UserViewModel>().Model;
        private IMobileServiceTable<Wiki> _wikiTable => Client.GetTable<Wiki>();
        private IMobileServiceTable<User> _userTable => Client.GetTable<User>();
        private IMobileServiceTable<Video> _videoTable => Client.GetTable<Video>();
        private IMobileServiceTable<Comment> _commentTable => Client.GetTable<Comment>();

        public async Task<Wiki> GetFullWiki(string id)
        {
            var result = await _wikiTable.LookupAsync(id);
            var sections = await Client.GetTable<WikiSection>().Where(s => s.WikiId == result.Id).ToListAsync();
            foreach (var section in sections)
            {
                result.Sections.Add(section);
            }
            return result;
        }
        // TODO:Implement actual login
        public async Task<User> Login(string loginId, string password)
        {
           var results = await _userTable.Where(u => u.QutId == loginId).ToListAsync();
            if (results.Count == 0)
                return null;
            else
                return await GetUser(results[0].Id);
        }

        public async Task<List<Leaderboard>> GetLeaderboard()
        {
            var results = await _userTable.OrderByDescending(u => u.Score).Take(50).ToListAsync();
            var leaderboard = results.Select(result => new Leaderboard()
            {
                UserName = result.Name, Score = result.Score
            }).ToList();
            return leaderboard;
        }

        public async Task<List<IFeedItem>> GetNewsfeed()
        {
            // Get relavant videos and wikis. Sort them by unit for now.
            var videoList = new List<Video>();
            var wikiList = new List<Wiki>();
            var videoTable = Client.GetTable<Video>();
            var wikiTable =  Client.GetTable<Wiki>();
            foreach (var unit in _user.Units)
            {
                var videos = await videoTable.Where(v => v.RelatedUnits.Contains(unit.Code)).Take(5).ToListAsync();
                videoList.AddRange(videos);
                var wikis = await wikiTable.Where(w => w.RelatedUnits.Contains(unit.Code)).Take(5).ToListAsync();
                wikiList.AddRange(wikis);
            }
            var feed = new List<IFeedItem>();
            feed.AddRange(videoList);
            feed.AddRange(wikiList);
            feed.Sort((x,y) => string.CompareOrdinal(x.RelatedUnits,y.RelatedUnits));
            return feed;
        }

        public async Task<bool> SaveComment(Comment comment)
        {
            try
            {
                var lookup = await _commentTable.Where(c => c.Id == comment.Id).ToListAsync();
                if (lookup.Count == 0)
                    await _commentTable.InsertAsync(comment);
                else
                    await _commentTable.UpdateAsync(comment);
            }
            catch (Exception e)
            {
                throw;
            }
            return true;
        }

        public async Task<User> GetUser(string userId)
        {
            try
            {
                var result = await _userTable.LookupAsync(userId);
                var achievements = await Client.GetTable<Achievement>().Where(a => a.UserId == result.Id).ToListAsync();
                var units = await Client.GetTable<Unit>().Where(a => a.UserId == result.Id).ToListAsync();
                result.Achievements.Clear();
                foreach (var achievement in achievements)
                {
                    result.Achievements.Add(achievement);
                }
                result.Units.Clear();
                foreach (var unit in units)
                {
                    result.Units.Add(unit);
                }
                return result;
            }
            catch (Exception e)
            {            
                throw;
            }
        }

        public async Task<List<Video>> GetVideos()
        {
            try
            {
                var result = await _videoTable.ToListAsync();
                return result;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public async Task<List<Wiki>> GetWikis()
        {
            try
            {
                var result = await _wikiTable.ToListAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveUser(User user)
        {
            try
            {
                var lookup = await _userTable.Where(u => u.Id == user.Id).ToListAsync();
                if (lookup.Count > 0)
                    await _userTable.UpdateAsync(user);
                // TODO: Update units and achievements
                else
                {
                    await _userTable.InsertAsync(user);
                    var achievements = Achievement.SeedAchievements();
                    foreach (var item in achievements)
                    {
                        // Set foreign key explicitly
                        item.UserId = user.Id;
                        await Client.GetTable<Achievement>().InsertAsync(item);
                    }
                    foreach (var unit in user.Units)
                    {
                        // No risks taken. Set foreign key explicitly
                        unit.UserId = user.Id;
                        await Client.GetTable<Unit>().InsertAsync(unit);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }

        public async Task<bool> SaveVideo(Video video)
        {
            try
            {
                var lookup = await _videoTable.Where(v => v.Id == video.Id).ToListAsync();
                if (lookup.Count != 0)
                    await _videoTable.UpdateAsync(video);
                else
                    await _videoTable.InsertAsync(video);
            }
            catch (Exception)
            {
                
                throw;
            }
            return true;

        }

        public Task<bool> UploadPicture(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
