namespace Shipwreck.ViewModelUtils;

public static class HasColumnsHelper
{
    public static IEnumerable<SelectionCommandViewModelBase> CreateColumnCommands(this IHasColumns page)
    {
        yield return new DefaultColumnsCommandViewModel(page);
        yield return new AllColumnsCommandViewModel(page);
        if (page is IHasColumnModes m)
        {
            foreach (var c in m.ModeCommands)
            {
                yield return c;
            }
        }
        yield return null;
        foreach (var kv in page.GetFlags())
        {
            var v = ((IConvertible)kv.Key).ToInt64(null);
            yield return new FlagColumnCommandViewModel(
                              page,
                              v,
                              kv.Value,
                              isSelected: (page.Columns & v) == v);
        }
    }

    public static IEnumerable<ExtensionColumnCommandViewModel> GetOrCreateExtensionColumnCommands(this IHasExtensionColumns page, IEnumerable<ExtensionColumnCommandViewModel> current)
    {
        var ret = new List<ExtensionColumnCommandViewModel>();
        foreach (var c in page.ExtensionColumns)
        {
            ret.Add(current?.FirstOrDefault(e => e.Value == c) ?? new ExtensionColumnCommandViewModel(page, c));
        }

        if (page is IHasColumnModes m)
        {
            m.OnExtensionsCreated(ret);
        }

        return ret;
    }
}
