using System;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Validation
{
    public sealed class RangeValidator<TModel, TProperty> : Validator<TModel, TProperty>
        where TModel : ValidatableModel
        where TProperty : IComparable<TProperty>
    {
        private TProperty _Minimum;
        private TProperty _Maximum;

        public RangeValidator(Expression<Func<TModel, TProperty>> expression, TProperty minimum, TProperty maximum, string errorMessage = null)
            : base(expression, errorMessage ?? $"{expression.GetDisplayName()}は{minimum}から{maximum}の間で指定してください。")
        {
            _Minimum = minimum;
            _Maximum = maximum;
        }

        protected override bool IsValid(TModel model, TProperty value)
            => _Minimum.CompareTo(value) <= 0 && value.CompareTo(_Maximum) <= 0;
    }
}
