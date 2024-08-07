﻿namespace Shipwreck.ViewModelUtils.Searching;

public class SearchPropertyViewModel : ObservableModel
{
    public SearchPropertyViewModel(SearchPropertyGroupViewModel group, QueryPropertyInfo model)
    {
        Group = group;
        Model = model;
        DisplayName = Host.GetDisplayName(model) ?? model.DisplayName;
    }

    public SearchPropertyGroupViewModel Group { get; }

    public ISearchPropertiesHost Host => Group.Host;

    public QueryPropertyInfo Model { get; }

    public string Name => Model.Name;
    public string DisplayName { get; }
    public string TypeName => Model.TypeName;
    public bool IsBoolean => Model is BooleanQueryPropertyInfo;
    public bool IsDateTime => Model is DateTimeQueryPropertyInfo;
    public bool IsEnum => Model is EnumQueryPropertyInfo;

    #region LocalName

    private string _LocalName;

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

    //public bool HasUniqueLocalName
    //    => _HasUniqueLocalName ??= Page.Properties.All(e => e != this || e.LocalName != LocalName);

    #endregion LocalName

    #region ConditionCount

    private int _ConditionCount;

    public int ConditionCount
    {
        get => _ConditionCount;
        private set => SetProperty(ref _ConditionCount, value);
    }

    internal void Invalidate()
        => ConditionCount = Host.Conditions.Count(e => e.Property == this);

    #endregion ConditionCount

    public bool IsDate => Model is DateTimeQueryPropertyInfo p && p.IsDate;
    public bool IsFlags => Model is EnumQueryPropertyInfo p && p.IsFlags;

    public bool AllowMultiple
        => !(Model is BooleanQueryPropertyInfo)
        && !(Model is EnumQueryPropertyInfo);

    public ConditionViewModel CreateCondition()
    {
        if (Host.TryCreateCondition(this, out var c)
            || (FrameworkPageViewModel.Handler is IFrameworkSearchPageViewModelHandler h
                && h.TryCreateCondition(this, out c)))
        {
            return c;
        }

        if (IsBoolean)
        {
            return new BooleanConditionViewModel(this);
        }
        if (IsDateTime)
        {
            return new DateTimeConditionViewModel(this);
        }
        if (IsEnum)
        {
            return new EnumConditionViewModel(this);
        }

        switch (TypeName)
        {
            case "Number":
            case nameof(SByte):
            case nameof(Byte):
            case nameof(Int16):
            case nameof(UInt16):
            case nameof(Int32):
            case nameof(UInt32):
            case nameof(Int64):
            case nameof(UInt64):
            case nameof(Single):
            case nameof(Double):
            case nameof(Decimal):
                return new NumberConditionViewModel(this);
        }

        return new StringConditionViewModel(this);
    }
}
