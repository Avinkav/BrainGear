using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using BrainGear.Data.Model;
using BrainGear.Data.ViewModel;
using GalaSoft.MvvmLight.Helpers;

namespace BrainGear.UI.Droid.Fragments
{
    public class EditWikiFragment : Fragment
    {
        private List<Binding<string, string>> _bindings = new List<Binding<string, string>>();
        private FloatingActionButton _fab;

        public WikiViewModel Vm { get; set; }

        public EditText SummaryText { get; set; }

        public EditText TitleText { get; set; }

        public List<EditText> TitleEditTexts { get; set; } = new List<EditText>();

        public List<EditText> SectionEditTexts { get; set; } = new List<EditText>();



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (Vm == null) throw new ArgumentNullException();
            var scrollView = new NestedScrollView(container.Context) { LayoutParameters = container.LayoutParameters };
            var layout = new LinearLayout(container.Context)
            {
                LayoutParameters = scrollView.LayoutParameters,
                Orientation = Orientation.Vertical
            };

            var view = inflater.Inflate(Resource.Layout.ItemEditWiki, layout, true);
            TitleText = view.FindViewById<EditText>(Resource.Id.editText1);
            SummaryText = view.FindViewById<EditText>(Resource.Id.editText2);

            _bindings.Add(this.SetBinding(() => Vm.Model.Title, () => TitleText.Text, BindingMode.TwoWay));
            _bindings.Add(this.SetBinding(() => Vm.Model.Title, () => SummaryText.Text, BindingMode.TwoWay));

            var counter = 0;
            foreach (var section in Vm.Model.Sections)
            {
                view = inflater.Inflate(Resource.Layout.ItemEditWiki, layout, true);
                TitleEditTexts.Add(view.FindViewById<EditText>(Resource.Id.editText1));
                SectionEditTexts.Add(view.FindViewById<EditText>(Resource.Id.editText2));

                // Attempted to bind. But setBinding doesn't work as edit texts are in a List. Don't see any other way of holding references to the edit texts as the number is dynamic.
                TitleEditTexts[counter].Text = section.Title;
                TitleEditTexts[counter].Text = section.Content;
                counter++;
            }
            scrollView.AddView(layout);
            return scrollView;
        }

        public void CommitEdits()
        {
            var sections = Vm.Model.Sections;

            for (var i = 0; i < TitleEditTexts.Count; i++)
            {
                if (i >= sections.Count) sections.Add(new WikiSection {Id = Guid.NewGuid().ToString()});
                sections[i].Position = i;
                sections[i].Title = TitleEditTexts[i].Text;
                sections[i].Content = SectionEditTexts[i].Text;
            }
        }

        //_bindings.Add(SectionEditTexts[counter].SetBinding(() => section.Content, () => SectionEditTexts[counter].Text, BindingMode.TwoWay));
        //_bindings.Add(TitleEditTexts[counter].SetBinding(() => section.Title, () => TitleEditTexts[counter].Text, BindingMode.TwoWay));
    }
}