using System;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Validation
{
    public sealed class RequiredValidator<TModel> : Validator<TModel, string>
        where TModel : ValidatableModel
    {
        public RequiredValidator(Expression<Func<TModel, string>> expression, string errorMessage = null)
            : base(expression, errorMessage ?? $"{expression.GetDisplayName()}は必須項目です。")
        {
        }

        protected override bool IsValid(TModel model, string value)
            => !string.IsNullOrEmpty(value);
    }
}
