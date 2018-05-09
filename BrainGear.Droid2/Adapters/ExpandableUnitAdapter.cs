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
using BrainGear.Data.Model;
using Object = Java.Lang.Object;

namespace BrainGear.UI.Droid.Adapters
{
    public class ExpandableUnitAdapter : BaseExpandableListAdapter
    {
        private readonly Context _context;

        private readonly List<GroupedUnit> _groupedUnits;

        public ExpandableUnitAdapter(Context context, List<GroupedUnit> groupedUnits)
        {
            _context = context;
            _groupedUnits = groupedUnits;
        }

        public override int GroupCount => _groupedUnits.Count;

        public override bool HasStableIds => true;

        public override Object GetChild(int groupPosition, int childPosition) => null;

        public override long GetChildId(int groupPosition, int childPosition) => childPosition;

        public override int GetChildrenCount(int groupPosition) => _groupedUnits[groupPosition].Units.Count;

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var view = convertView ??
                       LayoutInflater.From(_context)
                           .Inflate(Android.Resource.Layout.SimpleExpandableListItem2, parent, false);
            var textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            textView.Text = _groupedUnits[groupPosition].Units[childPosition].Code;
            return view;
        }

        public override Object GetGroup(int groupPosition) => null;

        public override long GetGroupId(int groupPosition) => groupPosition;


        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var view = convertView ??
                   LayoutInflater.From(_context)
                       .Inflate(Android.Resource.Layout.SimpleExpandableListItem1, parent, false);
            var group = _groupedUnits[groupPosition];
            var textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            textView.Text = $"{group.Year} Semester {group.Period}";
            var expandableListView = (ExpandableListView)parent;
            expandableListView.ExpandGroup(groupPosition);
            return view;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition) => true;
    }
}