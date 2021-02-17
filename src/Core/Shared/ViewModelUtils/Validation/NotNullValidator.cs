using System;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Validation
{
    public sealed class NotNullValidator<TModel, TProperty> : Validator<TModel, TProperty>
        where TModel : ValidatableModel
        where TProperty : class
    {
        public NotNullValidator(Expression<Func<TModel, TProperty>> expression, string errorMessage = null)
            : base(expression, errorMessage ?? $"{expression.GetDisplayName()}は必須項目です。")
        {
        }

        protected override bool IsValid(TModel model, TProperty value)
            => value != null;
    }
}
