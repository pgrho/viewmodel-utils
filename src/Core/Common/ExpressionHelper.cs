using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils;

internal static class ExpressionHelper
{
    internal static string GetPropertyName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
        => ((MemberExpression)expression.Body).Member.Name;

    internal static string GetDisplayName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
        => ((MemberExpression)expression.Body).Member.GetCustomAttribute<DisplayAttribute>()?.GetName()
            ?? ((MemberExpression)expression.Body).Member.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
            ?? ((MemberExpression)expression.Body).Member.Name;

    internal static Func<CommandViewModelBase, T> AddCommandArgument<T>(this Func<T> func)
        => func != null ? _ => func() : null;
}
