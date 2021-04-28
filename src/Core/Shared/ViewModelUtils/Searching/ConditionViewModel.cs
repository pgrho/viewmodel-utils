using System;
using System.Text;

namespace Shipwreck.ViewModelUtils.Searching
{
    public abstract partial class ConditionViewModel : ObservableModel
    {
        protected ConditionViewModel(SearchPropertyViewModel property)
        {
            Property = property;
        }

        public IFrameworkSearchPageViewModel Page => Property.Page;

        public SearchPropertyViewModel Property { get; }

        public string Name => Property.Model.Name;
        public string DisplayName => Property.DisplayName;

        public abstract bool HasValue { get; }

        public void SetValue(string value)
            => SetValue(null, value);

        public abstract void SetValue(string @operator, string value);

        public void SetValue<T>(string @operator, T value)
            where T : Enum
            => SetValue(@operator, value.ToString("D"));

        public virtual void Clear()
            => SetValue(null, null);

        public void Append(StringBuilder builder)
        {
            if (HasValue)
            {
                builder.Append(Property.Model.Name);
                builder.Append(':');
                AppendValueTo(builder);
            }
        }

        public abstract void AppendValueTo(StringBuilder builder);

        public void Remove() => Page.Conditions.Remove(this);

        #region RemoveCommand

        private CommandViewModelBase _RemoveCommand;

        public CommandViewModelBase RemoveCommand
            => _RemoveCommand ??= CommandViewModel.Create(Remove);

        #endregion RemoveCommand
    }
}
