using System;
using System.ComponentModel;
using System.Linq;

namespace Shipwreck.ViewModelUtils
{
    public sealed class DefaultColumnsCommandViewModel : FlagColumnCommandViewModel
    {
        private readonly long _AllFlags;
        private readonly IHasExtensionColumns _Extensions;

        public DefaultColumnsCommandViewModel(IHasColumns page)
            : this(page, page.GetFlags().Aggregate(0L, (s, kv) => s | ((IConvertible)kv.Key).ToInt64(null)))
        {
        }

        private DefaultColumnsCommandViewModel(IHasColumns page, long allFlags)
            : base(page,
                  page.DefaultColumns & allFlags,
                  SR.SelectDefault,
                  isSelected: (page.Columns & page.DefaultColumns & allFlags) == (page.DefaultColumns & allFlags),
                  unselectValue: ~page.DefaultColumns)
        {
            _AllFlags = allFlags;
            _Extensions = page as IHasExtensionColumns;
            if (_Extensions != null)
            {
                OnColumnsChanged();
            }
        }

        protected override void OnExecute()
        {
            Value = Page.DefaultColumns & _AllFlags;
            UnselectValue = ~Page.DefaultColumns;
            Page.Columns = Page.DefaultColumns;
            if (_Extensions != null)
            {
                _Extensions.SelectedExtensionColumns = _Extensions.GetDefaultExtensionColumns().ToList();
            }
            IsSelected = true;
        }

        protected override void OnPagePropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPagePropertyChanged(e);
            if (!IsExecuting
                && _Extensions != null
                && e.PropertyName == nameof(IHasExtensionColumns.SelectedExtensionColumns))
            {
                OnColumnsChanged();
            }
        }

        protected override void OnColumnsChanged()
        {
            if (_Extensions == null)
            {
                base.OnColumnsChanged();
            }
            else
            {
                IsSelected = (Page.Columns & (Value | UnselectValue)) == Value
                    && _Extensions.SelectedExtensionColumns.OrderBy(e => e).Distinct().SequenceEqual(
                            _Extensions.GetDefaultExtensionColumns().OrderBy(e => e).Distinct());
            }
        }
    }
}
