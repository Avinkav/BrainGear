using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using BrainGear.Data.Model;
using GalaSoft.MvvmLight;

namespace BrainGear.Data.Model
{
    public class User : ObservableObject
    {
        private string _course;
        private string _email;
        private string _name;
        //private string _nickname;
        private int _score;
        private string _profilePicUrl;

        public User()
        {
            Units = new ObservableCollection<Unit>();
            Achievements = new ObservableCollection<Achievement>();
        }

        public string Id { get;  set; }

        public string QutId { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => Name, ref _name, value);
                ;
            }
        }

        public string Email
        {
            get { return _email; }
            set { Set(() => Email, ref _email, value); }
        }

        //public string Nickname
        //{
        //    get { return _nickname; }
        //    set
        //    {
        //        Set(() => _nickname, ref _nickname, value);
        //        ;
        //    }
        //}

        public string Course
        {
            get { return _course; }
            set
            {
                Set(() => Course, ref _course, value);
                ;
            }
        }

        public int Score
        {
            get { return _score; }
            set { Set(() => Score, ref _score, value); }
        }

        // Raising property changed always as image needs to updated everytime URL is set. 
        // Even if the URL hasn't changed, the image at the URL may change.
        public string ProfilePicUrl
        {
            get { return _profilePicUrl; }
            set
            {
                _profilePicUrl = value;
                RaisePropertyChanged(() => ProfilePicUrl);
            }
        }

        [IgnoreDataMember]
        public ObservableCollection<Unit> Units { get; set; }

        [IgnoreDataMember]
        public ObservableCollection<Achievement> Achievements { get; set; }

        // Groups Unit by semester as required by UI
        public List<GroupedUnit> GroupedUnits => Units.GroupBy(u => u.Semester)
            .Select(grp => new GroupedUnit(){ Year = grp.Key.Split('/')[0], Period = grp.Key.Split('/')[1], Units = grp.ToList()})
            .ToList();

    }


    public class GroupedUnit
    {
        public string Year { get; set; }

        public string Period { get; set; }

        public List<Unit> Units { get; set; }
    }
}