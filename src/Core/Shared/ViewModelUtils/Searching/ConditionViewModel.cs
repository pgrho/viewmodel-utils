﻿namespace Shipwreck.ViewModelUtils.Searching;

public abstract partial class ConditionViewModel : ObservableModel
{
    protected ConditionViewModel(SearchPropertyViewModel property)
    {
        Property = property;
    }

    public ISearchPropertiesHost Host => Property.Host;

    public SearchPropertyViewModel Property { get; }

    public string Name => Property.Model.Name;
    public string DisplayName => Property.DisplayName;

    public abstract bool HasValue { get; }

    public void SetValue(string value)
        => SetValue(null, value);

    public abstract void SetValue(string @operator, string value);

    public void SetValue<T>(string @operator, T value)
        where T : Enum
        => SetValue(@operator, value.ToString("D"));

    public virtual void Clear()
        => SetValue(null, null);

    public void Append(StringBuilder builder)
    {
        if (HasValue)
        {
            builder.Append(Property.Model.Name);
            builder.Append(':');
            AppendValueTo(builder);
        }
    }

    public abstract void AppendValueTo(StringBuilder builder);

    public void Remove()
    {
        var hasValue = HasValue;
        Host.Conditions.Remove(this);

        var sameNames = Host.Conditions.Where(e => e.DisplayName == DisplayName).ToList();
        foreach (var oc in sameNames)
        {
            oc.ShouldShowPath = sameNames.Count > 1;
        }

        Property.Invalidate();

        if (hasValue)
        {
            Host.OnConditionChanged(this);
        }
    }

    #region ShouldShowPath

    private bool _ShouldShowPath;

    public bool ShouldShowPath
    {
        get => _ShouldShowPath;
        internal set => SetProperty(ref _ShouldShowPath, value);
    }

    #endregion ShouldShowPath

    #region ExtraCommands

    private CommandViewModelCollection _ExtraCommands;

    public CommandViewModelCollection ExtraCommands
        => _ExtraCommands ??= new(Host.CreateConditionCommands(this) ?? Enumerable.Empty<CommandViewModelBase>());

    #endregion ExtraCommands

    #region RemoveCommand

    private CommandViewModelBase _RemoveCommand;

    public CommandViewModelBase RemoveCommand
        => _RemoveCommand
        ??= CommandViewModel.Create(
            Remove,
            icon: "fas fa-times",
            style: BorderStyle.OutlineDanger);

    #endregion RemoveCommand

    public abstract bool TryCreateDefaultValueExpression(out string @operator, out string defaultValue);

    public virtual void SetDefaultValueExpression(string @operator, string defaultValue)
        => SetValue(@operator, defaultValue);
}
