using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using BrainGear.UI.Droid.Fragments;
using Java.Lang;

namespace BrainGear.UI.Droid.Adapters
{
    class VideosPagerAdapter : FragmentPagerAdapter
    {
        private readonly List<Type> _fragmentTypes = new List<Type>();

        public VideosPagerAdapter(FragmentManager fm)
            : base(fm)
        {
            _fragmentTypes.Add(typeof(UnitsFragment));
            _fragmentTypes.Add(typeof(RelatedVideosFragment));
            //_fragmentTypes.Add(typeof(AddVideoFragment));
        }
 
    public override Fragment GetItem(int position)
    {
       return (Fragment) Activator.CreateInstance(_fragmentTypes[position]);
    }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            switch (position)
            {
                case 0:
                    return new Java.Lang.String("Units");
                case 1:
                    return new Java.Lang.String("Related Videos");
                case 2:
                    return new Java.Lang.String("Upload");
                default:
                    return new Java.Lang.String("");
            }
        }

        public override int Count => _fragmentTypes.Count;
    }
}
