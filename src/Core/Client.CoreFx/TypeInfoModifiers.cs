using System.Linq.Expressions;
using System.Text.Json.Serialization.Metadata;

namespace Shipwreck.ViewModelUtils.Client;

public static class TypeInfoModifiers
{
    public static void SupportShouldSerialize(JsonTypeInfo typeInfo, Action<JsonTypeInfo, JsonPropertyInfo, Exception> exceptionCallback = null)
    {
        bool? isDataContract = null;
        foreach (var p in typeInfo.Properties)
        {
            if (p.ShouldSerialize != null)
            {
                continue;
            }

            var pi = (p.AttributeProvider as PropertyInfo);

            if ((isDataContract ??= pi?.ReflectedType.GetCustomAttribute<DataContractAttribute>() != null) == true)
            {
                if (pi?.GetCustomAttribute<DataMemberAttribute>() == null)
                {
                    p.ShouldSerialize = (_, _) => false;
                    continue;
                }
            }

            var m = typeInfo.Type.GetMethod("ShouldSerialize" + pi?.Name ?? p.Name, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            if (m != null)
            {
                var obj = Expression.Parameter(typeof(object), "obj");
                var value = Expression.Parameter(typeof(object), "value");

                var b = Expression.OrElse(
                    Expression.Not(Expression.TypeIs(obj, m.ReflectedType)),
                    Expression.Call(Expression.Convert(obj, m.ReflectedType), m));

                p.ShouldSerialize = Expression.Lambda<Func<object, object, bool>>(b, obj, value).Compile();
            }
            else if (pi?.GetCustomAttribute<DefaultValueAttribute>() is DefaultValueAttribute dva)
            {
                try
                {
                    var dv = dva.Value;

                    if (dv == null)
                    {
                        p.ShouldSerialize = (_, v) => v != null;
                    }
                    else
                    {
                        var dvt = dv.GetType();

                        if (!p.PropertyType.IsAssignableFrom(dvt) && dv is IConvertible conv)
                        {
                            dv = conv.ToType(Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType, null);
                            dvt = dv.GetType();
                        }

                        if (p.PropertyType.IsAssignableFrom(dvt))
                        {
                            var obj = Expression.Parameter(typeof(object), "obj");
                            var value = Expression.Parameter(typeof(object), "value");

                            var b = Expression.OrElse(
                                        Expression.Not(Expression.TypeIs(obj, pi.ReflectedType)),
                                        Expression.NotEqual(
                                            Expression.Constant(dv, p.PropertyType),
                                            Expression.Property(Expression.Convert(obj, pi.ReflectedType), pi)));

                            p.ShouldSerialize = Expression.Lambda<Func<object, object, bool>>(b, obj, value).Compile();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (exceptionCallback != null)
                    {
                        exceptionCallback(typeInfo, p, ex);
                    }
                    else
                    {
                        Console.Error?.WriteLine($"An exception is caught while setting ShouldSerialize of {typeInfo.Type.FullName}#{p.Name}.");
                        Console.Error?.WriteLine(ex);
                    }
                }
            }
        }
    }
}
