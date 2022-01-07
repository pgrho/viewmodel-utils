using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Validation;

public sealed class RequiredValidator<TModel> : Validator<TModel, string>
    where TModel : ValidatableModel
{
    public RequiredValidator(Expression<Func<TModel, string>> expression, string errorMessage = null)
        : base(expression, errorMessage ?? string.Format(SR.Arg0IsRequired, expression.GetDisplayName()))
    {
    }

    protected override bool IsValid(TModel model, string value)
        => !string.IsNullOrEmpty(value);
}
