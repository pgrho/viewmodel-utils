﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils.Components
{
    public abstract partial class ListComponentBase<T> : BindableComponentBase
        where T : class
    {
        #region Source

        private IList _Source;

        [Parameter]
        public IList Source
        {
            get => _Source;
            set
            {
                var prev = _Source;
                if (value != prev)
                {
                    using (BeginUpdate())
                    {
                        if (prev != null)
                        {
                            if (prev is INotifyCollectionChanged nc)
                            {
                                nc.CollectionChanged -= Source_CollectionChanged;
                            }
                            if (prev is INotifyPropertyChanged np)
                            {
                                np.PropertyChanged -= ListComponentBase_PropertyChanged;
                            }

                            foreach (var e in prev)
                            {
                                OnItemRemoved((T)e);
                            }
                        }
                        _Source = value;
                        if (value != null)
                        {
                            foreach (var e in value)
                            {
                                OnItemAdded((T)e);
                            }

                            if (value is INotifyCollectionChanged nc)
                            {
                                nc.CollectionChanged += Source_CollectionChanged;
                            }
                            if (value is INotifyPropertyChanged np)
                            {
                                np.PropertyChanged += ListComponentBase_PropertyChanged;
                            }
                        }
                    }
                }
            }
        }

        private List<T> _Handled;

        protected virtual void OnItemAdded(T item)
        {
            if (item is INotifyPropertyChanged n)
            {
                _Handled = _Handled ?? new List<T>();
                if (!_Handled.Contains(item))
                {
                    n.PropertyChanged += Item_PropertyChanged;
                    _Handled.Add(item);
                }
            }
        }

        protected virtual void OnItemRemoved(T item)
        {
            if (item is INotifyPropertyChanged n)
            {
                n.PropertyChanged -= Item_PropertyChanged;
                _Handled.Remove(item);
            }
        }

        protected virtual void OnReset()
        {
            if (_Handled != null)
            {
                foreach (var item in _Handled.ToArray())
                {
                    OnItemRemoved(item);
                }
            }

            foreach (var m in Source)
            {
                OnItemAdded((T)m);
            }
        }

        private void Source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_Source != sender)
            {
                return;
            }

            OnCollectionChanged(e);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BeginUpdate())
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems != null)
                        {
                            foreach (T m in e.NewItems)
                            {
                                OnItemAdded(m);
                            }
                            StateHasChanged();
                            return;
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems != null)
                        {
                            foreach (T m in e.OldItems)
                            {
                                OnItemRemoved(m);
                            }
                            StateHasChanged();
                            return;
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        if (e.OldItems != null)
                        {
                            foreach (T m in e.OldItems)
                            {
                                OnItemRemoved(m);
                            }

                            if (e.NewItems != null)
                            {
                                foreach (T m in e.NewItems)
                                {
                                    OnItemAdded(m);
                                }
                                StateHasChanged();
                                return;
                            }
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        StateHasChanged();
                        return;
                }

                OnReset();

                StateHasChanged();
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (OnItemPropertyChanged((T)sender, e.PropertyName))
            {
                StateHasChanged();
            }
        }

        private void ListComponentBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (OnSourcePropertyChanged(e.PropertyName))
            {
                StateHasChanged();
            }
        }

        [Parameter]
        public IEnumerable<string> DependsOnItemProperties { get; set; }

        [Parameter]
        public IEnumerable<string> IgnoresItemProperties { get; set; }

        protected virtual bool OnItemPropertyChanged(T item, string propertyName)
        => (DependsOnItemProperties == null && IgnoresItemProperties == null)
            || (DependsOnItemProperties?.Contains(propertyName) != false && IgnoresItemProperties?.Contains(propertyName) != true);

        protected virtual bool OnSourcePropertyChanged(string propertyName)
            => propertyName != nameof(Source.Count) && propertyName != "[]";

        #endregion Source
    }

    public abstract partial class ListComponentBase<TDataContext, TItem> : ListComponentBase<TItem>, IBindableComponent
        where TDataContext : class
        where TItem : class
    {
        object IBindableComponent.DataContext => DataContext;
    }
}
