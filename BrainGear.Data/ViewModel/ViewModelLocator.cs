/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:BrainGear.UI.Droid"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using BrainGear.Data.DataServices;
using BrainGear.Data.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace BrainGear.Data.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public const string WIKI_DETAILS  = "Wiki Details";
        public const string MAIN = "Root";
        public const string NEWSFEED = "Newsfeed";
        public const string VIDEOS = "Videos";
        public const string WIKI = "Wiki";
        public const string PROFILE = "User";
        public const string VIDEO_DETAILS = "Video Player";
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<IDataService, AzureDataService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<VideosViewModel>();
            SimpleIoc.Default.Register<WikisViewModel>();
            SimpleIoc.Default.Register<NewsfeedViewModel>();
            SimpleIoc.Default.Register<LeaderboardViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public NewsfeedViewModel Newsfeed => ServiceLocator.Current.GetInstance<NewsfeedViewModel>();

        public VideosViewModel Videos => ServiceLocator.Current.GetInstance<VideosViewModel>();

        public WikisViewModel Wiki => ServiceLocator.Current.GetInstance<WikisViewModel>();

        public UserViewModel User => ServiceLocator.Current.GetInstance<UserViewModel>();

        public LeaderboardViewModel Leaderboard => ServiceLocator.Current.GetInstance<LeaderboardViewModel>();

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
            SimpleIoc.Default.Reset();
        }
    }
}