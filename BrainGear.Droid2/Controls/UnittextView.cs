using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BrainGear.Controls
{
    class UnittextView : TextView
    {
        public UnittextView(Context context)
            : base(context)
        {

        }

        public UnittextView(Context context, string text)
            : base(context)
        {
            this.Gravity = GravityFlags.Center;
            this.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
            
            this.Text = text;

        }
    }
}