// ****************************************************************************
// <copyright file="ObservableAdapter.cs" company="GalaSoft Laurent Bugnion">
// Copyright � GalaSoft Laurent Bugnion 2009-2015
// </copyright>
// ****************************************************************************
// <author>Laurent Bugnion</author>
// <email>laurent@galasoft.ch</email>
// <date>02.10.2014</date>
// <project>GalaSoft.MvvmLight</project>
// <web>http://www.mvvmlight.net</web>
// <license>
// See license.txt in this solution or http://www.galasoft.ch/license_MIT.txt
// </license>
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace BrainGear.UI.Droid.Helpers
{
    /// <summary>
    /// A modified version of ObservableAdapter to work with Recycler Views
    /// </summary>
    /// <typeparam name="T">The type of the items contained in the <see cref="DataSource"/>.</typeparam>
    public class ObservableRecylcerAdapter<T> : RecyclerView.Adapter
    {

        public event EventHandler<int> ItemClick;
        protected IList<T> _list;
        protected INotifyCollectionChanged _notifier;

        public override int ItemCount => _list?.Count ?? 0;

        public void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public Action<RecyclerView.ViewHolder, int, T> BindFunc { get; set; }

        public Func<ViewGroup, int, RecyclerView.ViewHolder> CreateFunc { get; set; }
        /// <summary>
        /// Gets or sets the list containing the items to be represented in the list control.
        /// </summary>
        public IList<T> DataSource
        {
            get
            {
                return _list;
            }
            set
            {
                if (Equals(_list, value))
                {
                    return;
                }

                if (_notifier != null)
                {
                    _notifier.CollectionChanged -= NotifierCollectionChanged;
                }

                _list = value;
                _notifier = _list as INotifyCollectionChanged;

                if (_notifier != null)
                {
                    _notifier.CollectionChanged += NotifierCollectionChanged;
                }
            }
        }

        /// <summary>
        /// Gets the item corresponding to the index in the DataSource.
        /// </summary>
        /// <param name="index">The index of the item that needs to be returned.</param>
        /// <returns>The item corresponding to the index in the DataSource</returns>
        public T this[int index] => (_list == null) ? default(T) : _list[index];

        /// <summary>
        /// Returns a unique ID for the item corresponding to the position parameter.
        /// In this implementation, the method always returns the position itself.
        /// </summary>
        /// <param name="position">The position of the item for which the ID needs to be returned.</param>
        /// <returns>A unique ID for the item corresponding to the position parameter.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            BindFunc(holder, position, _list[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return CreateFunc(parent, viewType);
        }

        private void NotifierCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged();
        }
    }

   
}