using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Validation;

public abstract class Validator<TModel, TProperty>
    where TModel : ValidatableModel
{
    private readonly Func<TModel, TProperty> _Func;
    private readonly string _PropertyName;
    private readonly string _ErrorMessage;

    protected Validator(Expression<Func<TModel, TProperty>> expression, string errorMessage)
    {
        _Func = expression.Compile();
        _PropertyName = expression.GetPropertyName();
        _ErrorMessage = errorMessage;
    }

    internal void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var m = (TModel)sender;
        if (e.PropertyName == _PropertyName)
        {
            Validate(m);
        }
    }

    internal void Validate(TModel m)
    {
        if (IsValid(m, _Func(m)))
        {
            m.RemoveError(_ErrorMessage, _PropertyName);
        }
        else
        {
            m.AddError(_ErrorMessage, _PropertyName);
        }
    }

    protected abstract bool IsValid(TModel model, TProperty value);
}
