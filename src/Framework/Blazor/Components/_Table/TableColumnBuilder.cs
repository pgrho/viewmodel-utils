using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Components;

public abstract class TableColumnBuilder
{
    public TableBuilder TableBuilder { get; set; }

    public LambdaExpression ValueExpression { get; set; }
    public IEnumerable Options { get; set; }
    public LambdaExpression OptionsExpression { get; set; }
    public string Header { get; set; }
    public string SortKey { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }

    public float? Width { get; set; }

    public bool? IsReadOnly { get; set; }
    public LambdaExpression IsReadOnlyExpression { get; set; }
    public LambdaExpression IsChangedExpression { get; set; }

    public abstract TableColumn Build();

    protected IEnumerable<PropertyInfo> EnumerateProperties()
    {
        var r = new List<PropertyInfo>();

        for (var me = ValueExpression?.Body as MemberExpression; me != null; me = me.Expression as MemberExpression)
        {
            if (me.Member is PropertyInfo p)
            {
                r.Add(p);
            }
            else
            {
                return Enumerable.Empty<PropertyInfo>();
            }
        }

        r.Reverse();

        return r;
    }

    protected virtual string GetHeader()
        => (Header ?? string.Join("/", EnumerateProperties().Select(e => e.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? e.Name))).TrimOrNull();

    protected virtual string GetSortKey()
        => (SortKey ?? string.Join(".", EnumerateProperties().Select(e => e.Name))).TrimOrNull();

    protected virtual string GetDescription()
        => (Description ?? EnumerateProperties().LastOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description).TrimOrNull();

    protected virtual string GetIcon()
        => (Icon ?? EnumerateProperties().LastOrDefault()?.GetCustomAttribute<IconAttribute>()?.Icon).TrimOrNull();

    protected virtual string GetWidth() => Width > 0 ? $"{Width:0.#######}rem" : null;

    protected virtual IEnumerable<KeyValuePair<string, string>> EnumerateCellStyles()
    {
        if (GetWidth() is var w && w?.Length > 0)
        {
            yield return KeyValuePair.Create("width", w);
        }
    }

    protected virtual Dictionary<string, object> GetAdditionalAttributes()
    {
        var dic = new Dictionary<string, object>();

        var styles = EnumerateCellStyles().ToList();

        if (styles.Any())
        {
            dic["style"] = string.Join(";", styles.Select(e => e.Key + ":" + e.Value));
        }

        return dic.Any() ? dic : null;
    }

    protected virtual Func<object, T> GetGetValueDelegate<T>()
    {
        var propertyType = typeof(T);
        var p = Expression.Parameter(typeof(object), "obj");
        var def = Expression.Default(propertyType);

        if (ValueExpression != null)
        {
            var pt = ValueExpression.Parameters[0].Type;

            return Expression.Lambda<Func<object, T>>(
                Expression.Condition(
                    Expression.TypeIs(p, pt),
                    Expression.Convert(
                        ValueExpression.Body.Replace(
                            ValueExpression.Parameters[0],
                            Expression.Convert(p, pt)),
                        propertyType),
                    def), p).Compile();
        }
        return Expression.Lambda<Func<object, T>>(def, p).Compile();
    }

    protected virtual Action<object, T> GetSetValueDelegate<T>()
    {
        var propertyType = typeof(T);
        var p = Expression.Parameter(typeof(object), "obj");
        var v = Expression.Parameter(propertyType, "v");

        if (ValueExpression?.Body is MemberExpression me
            && me.Member is PropertyInfo pi && pi.CanWrite)
        {
            var pt = ValueExpression.Parameters[0].Type;

            return Expression.Lambda<Action<object, T>>(
                Expression.IfThen(
                    Expression.TypeIs(p, pt),
                    Expression.Assign(
                        ValueExpression.Body.Replace(
                            ValueExpression.Parameters[0],
                            Expression.Convert(p, pt)),
                        v)),
                p, v).Compile();
        }

        return Expression.Lambda<Action<object, T>>(
            Expression.Empty(),
            p,
            v).Compile();
    }

    protected virtual Func<object, IEnumerable<T>> GetOptionsDelegate<T>()
    {
        if (Options == null && OptionsExpression == null)
        {
            return null;
        }
        var p = Expression.Parameter(typeof(object), "obj");
        var def = Expression.Constant(Options ?? Enumerable.Empty<T>(), typeof(IEnumerable<T>));

        if (OptionsExpression != null)
        {
            var pt = OptionsExpression.Parameters[0].Type;

            return Expression.Lambda<Func<object, IEnumerable<T>>>(
                Expression.Condition(
                    Expression.TypeIs(p, pt),
                    Expression.Convert(
                        OptionsExpression.Body.Replace(
                            OptionsExpression.Parameters[0],
                            Expression.Convert(p, pt)),
                        typeof(IEnumerable<T>)),
                    def), p).Compile();
        }

        return Expression.Lambda<Func<object, IEnumerable<T>>>(def, p).Compile();
    }

    protected virtual Func<object, bool> GetIsReadOnlyDelegate()
    {
        if (IsReadOnly != null)
        {
            return GetBooleanDelegate(IsReadOnly.Value);
        }
        if (IsReadOnlyExpression != null)
        {
            return GetBooleanDelegate(IsReadOnlyExpression);
        }
        if (TableBuilder.DefaultIsReadOnly != null)
        {
            return GetBooleanDelegate(TableBuilder.DefaultIsReadOnly.Value);
        }
        if (TableBuilder.DefaultIsReadOnlyExpression != null)
        {
            return GetBooleanDelegate(TableBuilder.DefaultIsReadOnlyExpression);
        }
        return GetBooleanDelegate(false);
    }

    protected virtual Func<object, bool> GetIsChangedDelegate()
    {
        if (IsChangedExpression != null)
        {
            return GetBooleanDelegate(IsChangedExpression);
        }

        return GetBooleanDelegate(false);
    }

    static Func<object, bool> GetBooleanDelegate(bool b)
            => b ? _ => true : _ => false;

    static Func<object, bool> GetBooleanDelegate(LambdaExpression expression)
    {
        var p = Expression.Parameter(typeof(object), "obj");
        var pt = expression.Parameters[0].Type;

        return Expression.Lambda<Func<object, bool>>(
            Expression.Condition(
                Expression.TypeIs(p, pt),
                expression.Body.Replace(
                    expression.Parameters[0],
                    Expression.Convert(p, pt)),
                Expression.Constant(false)), p).Compile();
    }
}
