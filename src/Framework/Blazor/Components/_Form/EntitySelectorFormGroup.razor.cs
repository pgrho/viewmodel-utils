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

    [Parameter]
    public bool IsNameVisible { get; set; } = true;

    [Parameter]
    public IEntitySelector DataContext { get; set; }

    [Parameter]
    public string AppendToSelector { get; set; } = ".body-root";
}
