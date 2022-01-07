using System.IO;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Validation;

public sealed class DirectoryPathValidator<TModel> : Validator<TModel, string>
    where TModel : ValidatableModel
{
    public DirectoryPathValidator(Expression<Func<TModel, string>> expression, string errorMessage = null)
        : base(expression, errorMessage ?? string.Format(SR.Arg0IsInvalidDirectory, expression.GetDisplayName()))
    {
    }

    protected override bool IsValid(TModel model, string value)
    {
        try
        {
            return string.IsNullOrEmpty(value) || Directory.Exists(value);
        }
        catch
        {
            return false;
        }
    }
}
