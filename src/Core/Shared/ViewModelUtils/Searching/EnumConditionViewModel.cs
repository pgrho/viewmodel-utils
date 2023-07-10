namespace Shipwreck.ViewModelUtils.Searching;

public sealed class EnumConditionViewModel : MultipleOptionConditionViewModel<EnumFieldInfo>
{
    public EnumConditionViewModel(SearchPropertyViewModel property)
        : base(property)
    {
    }

    private EnumQueryPropertyInfo Model => (EnumQueryPropertyInfo)Property.Model;

    protected override void InitializeOptions()
    {
        Options.Set(
            Model.Fields
                .Where(e => !Model.IsFlags || e.Value != 0)
                .Select(e => new EnumOptionViewModel(
                    this, e, e.DisplayName ?? e.Name, isSelected: true)));
        IsSearching = false;
    }

    public override void SetValue(string @operator, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            foreach (var op in Options)
            {
                op.IsSelected = true;
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

        if (Model.IsFlags)
        {
            if (long.TryParse(value, out var v))
            {
                foreach (var op in Options)
                {
                    op.IsSelected = (v & op.Value.Value) != 0;
                }
            }
        }
        else
        {
            var vs = value.Split(',')
                    .Select(e => long.TryParse(e, out var l) ? l : (long?)null)
                    .Where(e => e != null)
                    .Select(e => e.Value)
                    .OrderBy(e => e)
                    .Distinct()
                    .ToList();

            var isInclude = @operator != "!=";
            foreach (var op in Options)
            {
                op.IsSelected = vs.Contains(op.Value.Value) == isInclude;
            }
        }
    }

    public override bool HasValue
    {
        get
        {
            if (Model.IsFlags)
            {
                return true;
            }
            else
            {
                var ids = Options.Where(e => e.IsSelected).Select(e => e.Value.Value).ToList();
                if (ids.Count > 0 && ids.Count < Options.Count)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public override void AppendValueTo(StringBuilder builder)
    {
        var ids = Options.Where(e => e.IsSelected).Select(e => e.Value.Value).ToList();

        if (Model.IsFlags)
        {
            builder.Append(ids.Any() ? ids.Aggregate((a, b) => a | b) : 0);
        }
        else
        {
            if (ids.Count > 0 && ids.Count < Options.Count)
            {
                foreach (var v in ids)
                {
                    builder.Append(v);
                    builder.Append(',');
                }
                builder.Length--;
            }
        }
    }

    public override bool TryCreateDefaultValueExpression(out string @operator, out string defaultValue)
    {
        @operator = null;
        if (HasValue)
        {
            var sb = new StringBuilder();
            AppendValueTo(sb);

            defaultValue = sb.ToString();

            return true;
        }
        defaultValue = null;

        return false;
    }
}
