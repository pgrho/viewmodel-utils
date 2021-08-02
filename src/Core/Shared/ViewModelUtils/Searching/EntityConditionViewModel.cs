using System.Text;

namespace Shipwreck.ViewModelUtils.Searching
{
    public abstract class EntityConditionViewModel : ConditionViewModel
    {
        protected EntityConditionViewModel(SearchPropertyViewModel property)
            : base(property)
        {
        }

        #region Selector

        private IEntitySelector _Selector;

        public IEntitySelector Selector
        {
            get
            {
                if (_Selector == null)
                {
                    _Selector = CreateSelector();
                    _Selector.PropertyChanged += _Selector_PropertyChanged;
                }
                return _Selector;
            }
        }

        public object SelectedId
        {
            get => Selector.SelectedId;
            set => Selector.SelectedId = value;
        }

        public object SelectedItem
        {
            get => Selector.SelectedItem;
            set => Selector.SelectedItem = value;
        }

        public bool IsSearching
            => Selector.IsSearching;

        private void _Selector_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Selector.SelectedId):
                    RaisePropertyChanged(nameof(SelectedId));
                    break;

                case nameof(Selector.SelectedItem):
                    RaisePropertyChanged(nameof(SelectedItem));
                    break;

                case nameof(Selector.IsSearching):
                    RaisePropertyChanged(nameof(IsSearching));
                    break;
            }
        }

        protected abstract IEntitySelector CreateSelector();

        #endregion Selector

        public override void SetValue(string @operator, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Selector.SelectedId = null;
                return;
            }

            if (string.IsNullOrEmpty(@operator))
            {
                if (value.StartsWith("="))
                {
                    @operator = "=";
                    value = value.Substring(1);
                }
            }

            if (int.TryParse(value, out var id) && id > 0)
            {
                Selector.SelectedId = id;
            }
            else
            {
                Selector.SelectedId = null;
            }
        }

        public override bool HasValue
            => Selector.IsValid(Selector.SelectedId);

        public override void AppendValueTo(StringBuilder builder)
            => builder.Append(Selector.SelectedId);

        public override bool TryCreateDefaultValueExpression(out string @operator, out string defaultValue)
        {
            @operator = null;
            defaultValue = Selector.SelectedId?.ToString();

            return defaultValue != null;
        }
    }
}
