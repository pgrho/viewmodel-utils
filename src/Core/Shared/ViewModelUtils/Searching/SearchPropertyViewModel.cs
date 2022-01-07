namespace Shipwreck.ViewModelUtils.Searching;

public class SearchPropertyViewModel : ObservableModel
{
    public SearchPropertyViewModel(IFrameworkSearchPageViewModel page, QueryPropertyInfo model)
    {
        Page = page;
        Model = model;
        DisplayName = page.GetPropertyDisplayName(model.Name) ?? model.DisplayName;
    }

    public IFrameworkSearchPageViewModel Page { get; }
    protected internal QueryPropertyInfo Model { get; }

    public string Name => Model.Name;
    public string DisplayName { get; }
    public string TypeName => Model.TypeName;
    public bool IsBoolean => Model is BooleanQueryPropertyInfo;
    public bool IsDateTime => Model is DateTimeQueryPropertyInfo;
    public bool IsEnum => Model is EnumQueryPropertyInfo;

    #region LocalName

    private string _LocalName;

    private bool? _HasUniqueLocalName;

    public string LocalName
    {
        get
        {
            if (_LocalName == null)
            {
                var li = Name.LastIndexOf('.');
                _LocalName = li < 0 ? Name : Name.Substring(li + 1);
            }
            return _LocalName;
        }
    }

    public bool HasUniqueLocalName
        => _HasUniqueLocalName ??= Page.Properties.All(e => e != this || e.LocalName != LocalName);

    #endregion LocalName

    #region Parent

    private bool _IsParentLoaded;

    private SearchPropertyViewModel _Parent;
    private string _AncestorPath;

    public SearchPropertyViewModel Parent
        => ResolveParent()._Parent;

    public string AncestorPath
        => ResolveParent()._AncestorPath;

    private SearchPropertyViewModel ResolveParent()
    {
        if (!_IsParentLoaded)
        {
            _IsParentLoaded = true;

            var li = Name.LastIndexOf('.');
            if (li >= 0)
            {
                var pn = Name.Substring(0, li);

                _Parent = Page.Properties.FirstOrDefault(e => e.Name == pn);

                StringBuilder sb = null;
                for (var p = _Parent; p != null; p = p.Parent)
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder(p.DisplayName);
                    }
                    else
                    {
                        sb.Insert(0, "/").Insert(0, p.DisplayName);
                    }
                }
                _AncestorPath = sb?.ToString();
            }
        }
        return this;
    }

    #endregion Parent

    public bool IsDate => Model is DateTimeQueryPropertyInfo p && p.IsDate;
    public bool IsFlags => Model is EnumQueryPropertyInfo p && p.IsFlags;

    public bool AllowMultiple
        => !(Model is BooleanQueryPropertyInfo)
        && !(Model is EnumQueryPropertyInfo);

    public ConditionViewModel CreateCondition()
        => Page?.CreateCondition(this);
}
