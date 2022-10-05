namespace Shipwreck.ViewModelUtils.Components;

public partial class EntitySelectorFormGroup
{
    #region EntitySelectorId

    private string _EntitySelectorId;

    [Parameter]
    public string EntitySelectorId
    {
        get => _EntitySelectorId ??= (FormGroupId + "--entityselector");
        set => SetProperty(ref _EntitySelectorId, value);
    }

    #endregion EntitySelectorId

    #region IsNameVisible

    private bool _IsNameVisible = true;

    [Parameter]
    public bool IsNameVisible
    {
        get => _IsNameVisible;
        set => SetProperty(ref _IsNameVisible, value);
    }

    #endregion IsNameVisible

    #region DataContext

    private IEntitySelector _DataContext;

    [Parameter]
    public IEntitySelector DataContext
    {
        get => _DataContext;
        set => SetProperty(ref _DataContext, value);
    }

    #endregion DataContext

    #region AppendToSelector

    private string _AppendToSelector = ".body-root";

    [Parameter]
    public string AppendToSelector
    {
        get => _AppendToSelector;
        set => SetProperty(ref _AppendToSelector, value);
    }

    #endregion AppendToSelector
}
