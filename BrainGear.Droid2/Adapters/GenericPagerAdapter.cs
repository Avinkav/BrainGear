using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using BrainGear.UI.Droid.Fragments;
using Java.Lang;

namespace BrainGear.UI.Droid.Adapters
{
    class GenericPagerAdapter : FragmentPagerAdapter
    {
        private readonly List<Fragment> _fragments;

        public GenericPagerAdapter(FragmentManager fm, List<Fragment> fragments)
            : base(fm)
        {
            _fragments = fragments;
        }

        public override Fragment GetItem(int position) => _fragments[position];

        public override ICharSequence GetPageTitleFormatted(int position) => new Java.Lang.String(_fragments[position].ToString());

        public override int Count => _fragments.Count;
    }
}
