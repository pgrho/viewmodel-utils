using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Validation;

public static class ValidationHelper
{
    public static TModel AddValidator<TModel, TProperty>(this TModel obj, Expression<Func<TModel, TProperty>> expression, Validator<TModel, TProperty> validator)
        where TModel : ValidatableModel
    {
        validator.Validate(obj);
        obj.PropertyChanged += validator.HandlePropertyChanged;
        return obj;
    }

    public static TModel Requires<TModel>(this TModel obj, Expression<Func<TModel, string>> expression)
        where TModel : ValidatableModel
        => obj.AddValidator(expression, new RequiredValidator<TModel>(expression));

    public static TModel Requires<TModel, TProperty>(this TModel obj, Expression<Func<TModel, TProperty>> expression)
        where TModel : ValidatableModel
        where TProperty : class
        => obj.AddValidator(expression, new NotNullValidator<TModel, TProperty>(expression));

    public static TModel IsDirectory<TModel>(this TModel obj, Expression<Func<TModel, string>> expression)
        where TModel : ValidatableModel
        => obj.AddValidator(expression, new DirectoryPathValidator<TModel>(expression));

    public static TModel IsGreaterThanOrEqual<TModel, TProperty>(this TModel obj, Expression<Func<TModel, TProperty>> expression, TProperty minimum)
        where TModel : ValidatableModel
        where TProperty : IComparable<TProperty>
        => obj.AddValidator(
                expression,
                new RangeValidator<TModel, TProperty>(
                        expression,
                        minimum,
                        (TProperty)typeof(TProperty).GetField(nameof(int.MaxValue)).GetValue(null),
                        string.Format(
                            SR.Arg0MustBeGreaterThanOrEqualToArg1,
                            expression.GetDisplayName(),
                            minimum)));
}
