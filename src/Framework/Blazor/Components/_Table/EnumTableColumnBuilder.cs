using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Components;

internal sealed class EnumTableColumnBuilder<T> : PropertyTableColumnBuilder
    where T : struct, Enum
{
    protected override Func<object, IEnumerable<TValue>> GetOptionsDelegate<TValue>()
    {
        var p = Expression.Parameter(typeof(object), "obj");
        var def = Expression.Constant(Options ?? Enum.GetValues(typeof(TValue)).Cast<TValue>().ToArray(), typeof(IEnumerable<TValue>));

        if (OptionsExpression != null)
        {
            var pt = OptionsExpression.Parameters[0].Type;

            return Expression.Lambda<Func<object, IEnumerable<TValue>>>(
                Expression.Condition(
                    Expression.TypeIs(p, pt),
                    Expression.Convert(
                        OptionsExpression.Body.Replace(
                            OptionsExpression.Parameters[0],
                            Expression.Convert(p, pt)),
                        typeof(IEnumerable<TValue>)),
                    def), p).Compile();
        }

        return Expression.Lambda<Func<object, IEnumerable<TValue>>>(def, p).Compile();
    }

    protected override TableColumn CreateTableColumn()
        => new EnumTableColumn<T>()
        {
            GetValueDelegate = GetGetValueDelegate<T>(),
            SetValueDelegate = GetSetValueDelegate<T>(),
            OptionsDelegate = GetOptionsDelegate<T>(),
        };
}
