namespace Shipwreck.ViewModelUtils.Searching;

public class SearchPropertyGroupViewModel : ObservableModel
{
    public SearchPropertyGroupViewModel(ISearchPropertiesHost host)
        : this(host, null, string.Empty, string.Empty, string.Empty)
    {
        _IsExpanded = true;
    }

    private SearchPropertyGroupViewModel(ISearchPropertiesHost host, SearchPropertyGroupViewModel parent, string path, string displayName, string typeName)
    {
        Host = host;
        Parent = parent;
        Path = path;
        DisplayName = displayName;
        DisplayNamePath = (parent != null ? parent.DisplayNamePath + "/" : null) + displayName;
        _TypeName = typeName;
    }

    public ISearchPropertiesHost Host { get; }

    public bool IsRoot => Parent == null;

    public SearchPropertyGroupViewModel Parent { get; }
    public string Path { get; }

    public string DisplayName { get; }

    public string DisplayNamePath { get; }

    private readonly string _TypeName;

    #region IsExpanded

    private bool _IsExpanded;

    public bool IsExpanded
    {
        get => _IsExpanded;
        set => SetProperty(ref _IsExpanded, value || IsRoot);
    }

    public void ToggleIsExpanded()
        => IsExpanded = !IsExpanded;

    #endregion IsExpanded

    #region Children

    private BulkUpdateableCollection<SearchPropertyGroupViewModel> _Children;

    public BulkUpdateableCollection<SearchPropertyGroupViewModel> Children
    {
        get
        {
            if (_Children == null)
            {
                _Children = new();
                var qs = Host.QuerySettings;

                var prefix = string.IsNullOrEmpty(Path) ? string.Empty : (Path + ".");

                Func<QueryGroupInfo, bool> predicate = string.IsNullOrEmpty(Path)
                    ? e => !e.Path.Contains('.')
                    : e => e.Path.StartsWith(prefix) && e.Path.IndexOf('.', prefix.Length) < 0;

                foreach (var g in qs?.Groups.Where(predicate) ?? [])
                {
                    if (!Host.ShouldInclude(g))
                    {
                        continue;
                    }
                    _Children.Add(new(Host, this, g.Path, Host.GetDisplayName(g) ?? g.DisplayName, g.TypeName));
                }
            }
            return _Children;
        }
    }

    #endregion Children

    #region Properties

    private BulkUpdateableCollection<SearchPropertyViewModel> _Properties;

    public BulkUpdateableCollection<SearchPropertyViewModel> Properties
    {
        get
        {
            if (_Properties == null)
            {
                _Properties = new();
                var qs = Host.QuerySettings;

                var t = qs?.Types.FirstOrDefault(e => e.TypeName == _TypeName);

                if (t?.Properties != null)
                {
                    foreach (var p in t.Properties)
                    {
                        if (!Host.ShouldInclude(p))
                        {
                            continue;
                        }

                        var sp = p;
                        if (!string.IsNullOrEmpty(Path))
                        {
                            sp = sp.Clone();
                            sp.Name = Path + "." + sp.Name;
                        }

                        _Properties.Add(new(this, sp));
                    }
                }
            }
            return _Properties;
        }
    }

    #endregion Properties

    public SearchPropertyViewModel FindProperty(string path)
        => FindProperty(path.Split('.'));

    public SearchPropertyViewModel FindProperty(IReadOnlyList<string> path)
    {
        if (path.Count == 0)
        {
            return null;
        }

        var n = path[0];
        if (path.Count == 1)
        {
            return Properties.FirstOrDefault(e => e.Name == n);
        }
        else
        {
            var p = !IsRoot ? Path + "." + n : n;
            return Children.FirstOrDefault(e => e.Path == p).FindProperty(path.Skip(1).ToList());
        }
    }
    public ConditionViewModel GetOrCreateCondition(string path, ConditionCreationBehavior behavior = ConditionCreationBehavior.CreateNew)
    {
        if (!IsRoot)
        {
            throw new InvalidOperationException();
        }

        return GetOrCreateCondition(FindProperty(path), behavior);
    }

    internal ConditionViewModel GetOrCreateCondition(SearchPropertyViewModel p, ConditionCreationBehavior behavior = ConditionCreationBehavior.CreateNew)
    {
        if (p == null)
        {
            return null;
        }

        var c = (behavior & ConditionCreationBehavior.PreferLast) != 0
            ? Host.Conditions.LastOrDefault(e => e.Property == p)
            : Host.Conditions.FirstOrDefault(e => e.Property == p);

        if ((behavior & ConditionCreationBehavior.CreateNew) == 0
            || ((!p.AllowMultiple || (behavior & ConditionCreationBehavior.PreferNew) == 0) && c != null))
        {
            return c;
        }

        var nc = p.CreateCondition();

        if (nc != null)
        {
            Host.Conditions.Add(nc);

            p.Invalidate();

            return nc;
        }

        return c;
    }

    // protected internal SearchPropertyGroupViewModel(SearchPropertiesModalViewModel modal, string ancestorPath, IEnumerable<SearchPropertyViewModel> properties, string displayName = null)
    // {
    //     Modal = modal;
    //     AncestorPath = ancestorPath;
    //     _IsExpanded = string.IsNullOrEmpty(ancestorPath);
    //     Items = Array.AsReadOnly(properties.ToArray());
    //     _DisplayName = displayName;
    // }

    // SearchPropertiesModalViewModel Modal { get; }

    // #region DisplayName

    // private string _DisplayName;

    // public string DisplayName
    //     => (_DisplayName ??= GetDisplayName()).TrimOrNull();

    //    protected virtual string GetDisplayName()
    //        => AncestorPath == null ? string.Empty
    //        : (Modal.Properties.FirstOrDefault(e => e.Name == AncestorPath)?.DisplayName ?? AncestorPath);

    //    #endregion DisplayName

    //    public string AncestorPath { get; }
}
