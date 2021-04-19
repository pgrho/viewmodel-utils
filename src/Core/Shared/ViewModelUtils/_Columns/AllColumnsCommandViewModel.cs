using System;
using System.ComponentModel;
using System.Linq;

namespace Shipwreck.ViewModelUtils
{
    public sealed class AllColumnsCommandViewModel : FlagColumnCommandViewModel
    {
        private readonly IHasExtensionColumns _Extensions;

        public AllColumnsCommandViewModel(IHasColumns page)
            : this(page, page.GetFlags().Aggregate(0L, (s, kv) => s | ((IConvertible)kv.Key).ToInt64(null)))
        {
        }

        private AllColumnsCommandViewModel(IHasColumns page, long allFlags)
            : base(page, allFlags, SR.SelectAll, isSelected: (page.Columns & allFlags) == allFlags)
        {
            _Extensions = page as IHasExtensionColumns;
            if (_Extensions != null)
            {
                OnColumnsChanged();
            }
        }

        protected override void OnExecute()
        {
            base.OnExecute();
            if (_Extensions != null)
            {
                _Extensions.SelectedExtensionColumns = _Extensions.ExtensionColumns.ToList();
            }
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
                    && !_Extensions.ExtensionColumns.Except(_Extensions.SelectedExtensionColumns).Any();
            }
        }
    }
}
