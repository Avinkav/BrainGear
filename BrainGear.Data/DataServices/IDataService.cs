using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;

namespace BrainGear.Data.DataServices
{
    public interface IDataService
    {

        Task<List<Video>> GetVideos();

        Task<bool> SaveVideo(Video video);

        Task<bool> SaveComment(Comment comment);

        Task<User> GetUser(string userId);

        Task<bool> SaveUser(User user);

        Task<List<IFeedItem>> GetNewsfeed();

        Task<bool> UploadPicture(byte[] bytes);

        Task<List<Leaderboard>> GetLeaderboard();

        Task<List<Wiki>> GetWikis();

        Task<Wiki> GetFullWiki(string id);

        Task<User> Login(string loginId, string password);
    }
}
