using GalaSoft.MvvmLight;

namespace BrainGear.Data.Model
{
    public class Leaderboard : ObservableObject
    {
        public string UserName { get; set; }

        public int Score { get; set; }

        public  int Rank { get; set; }
    }
}