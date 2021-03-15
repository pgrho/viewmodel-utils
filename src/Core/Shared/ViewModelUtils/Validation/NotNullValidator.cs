using System;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Validation
{
    public sealed class NotNullValidator<TModel, TProperty> : Validator<TModel, TProperty>
        where TModel : ValidatableModel
        where TProperty : class
    {
        public NotNullValidator(Expression<Func<TModel, TProperty>> expression, string errorMessage = null)
            : base(expression, errorMessage ?? string.Format(SR.Arg0IsRequired, expression.GetDisplayName()))
        {
        }

        protected override bool IsValid(TModel model, TProperty value)
            => value != null;
    }
}
