namespace Shipwreck.ViewModelUtils.Searching;

public abstract class EntitiesConditionViewModel : MultipleOptionConditionViewModel<object>
{
    protected EntitiesConditionViewModel(SearchPropertyViewModel property)
        : base(property)
    {
    }

    #region Selector

    private IEntitySelector _Selector;

    public IEntitySelector Selector
    {
        get
        {
            if (_Selector == null)
            {
                var s = CreateSelector();
                if (!s.UseList)
                {
                    throw new InvalidOperationException("IEntitySelector.UseList must be true");
                }
                _Selector = s;
            }
            return _Selector;
        }
    }

    protected abstract IEntitySelector CreateSelector();

    #endregion Selector

    #region Options

    private Task<BulkUpdateableCollection<MultipleOptionViewModel<object>>> _OptionsTask;
    private bool _IsInclude;
    private object[] _Ids;

    protected override void InitializeOptions()
    {
        _OptionsTask = InitializeOptionsCore();
    }

    private async Task<BulkUpdateableCollection<MultipleOptionViewModel<object>>> InitializeOptionsCore()
    {
        IsSearching = true;
        try
        {
            var ops = await Selector.GetItemsTask().ConfigureAwait();
            Options.Set(
                ops.Cast<object>()
                    .Select(e => new MultipleOptionViewModel<object>(
                    this,
                    e,
                    Selector.GetDisplayText(e),
                    _Ids?.Contains(Selector.GetId(e)) == _IsInclude)));
            _Ids = null;
            DisplayText = null;
        }
        catch { }
        finally
        {
            IsSearching = false;
        }
        return Options;
    }

    #endregion Options

    protected override string GetDisplayText()
    {
        if (_OptionsTask?.Status != TaskStatus.RanToCompletion)
        {
            if (_Ids?.Length > 0)
            {
                if (_IsInclude)
                {
                    return string.Format(SR.CountArg0Items, _Ids.Length);
                }
                else
                {
                    return string.Format(
                        SR.ExceptForArg0,
                        string.Format(SR.CountArg0Items, _Ids.Length));
                }
            }
        }
        return base.GetDisplayText();
    }

    public override bool HasValue
    {
        get
        {
            if (_OptionsTask?.Status == TaskStatus.RanToCompletion)
            {
                if (Options.Any())
                {
                    var ids = Options.Where(e => e.IsSelected).Select(e => Selector.GetId(e.Value)).ToList();
                    if (ids.Count < Options.Count)
                    {
                        return true;
                    }
                }
            }
            else if (_Ids?.Length > 0)
            {
                return true;
            }
            return false;
        }
    }

    public override void AppendValueTo(StringBuilder builder)
    {
        void appendCore(string @operator, IEnumerable<object> ids)
        {
            var ol = builder.Length;
            builder.Append(@operator);
            var pl = builder.Length;
            foreach (var id in ids)
            {
                builder.Append(id);
                builder.Append(',');
            }
            if (builder.Length == pl)
            {
                builder.Length = ol;
            }
            else
            {
                builder.Length--;
            }
        }
        if (_OptionsTask?.Status == TaskStatus.RanToCompletion)
        {
            if (Options.Any())
            {
                var ids = Options.Where(e => e.IsSelected).Select(e => Selector.GetId(e.Value)).ToList();
                if (ids.Count < Options.Count)
                {
                    appendCore(null, ids);
                }
            }
        }
        else if (_Ids?.Length > 0)
        {
            appendCore(_IsInclude ? null : "!=", _Ids);
        }
    }

    public override void SetValue(string @operator, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _Ids = null;
            foreach (var op in Options)
            {
                op.IsSelected = false;
            }
            return;
        }

        if (string.IsNullOrEmpty(@operator))
        {
            if (value.StartsWith("="))
            {
                @operator = "=";
                value = value.Substring(1);
            }
            else if (value.StartsWith("!="))
            {
                @operator = "!=";
                value = value.Substring(2);
            }
        }

        var vs = value.Split(',')
            .Select(e => Selector.TryParseId(e, out var i) ? i : null)
            .Where(e => e != null)
            .Distinct()
            .ToArray();

        var include = @operator != "!=";
        if (_OptionsTask?.Status == TaskStatus.RanToCompletion)
        {
            foreach (var op in Options)
            {
                op.IsSelected = vs.Contains(Selector.GetId(op.Value)) == include;
            }
        }
        else if (vs.Any())
        {
            _Ids = vs;
            _IsInclude = include;
            Options.GetHashCode();
            DisplayText = null;
        }
    }
}
