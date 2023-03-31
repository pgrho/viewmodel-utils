namespace Shipwreck.ViewModelUtils.Components;

public abstract class PopoverRowBase : BindableComponentBase
{
    protected override bool ImplicitRender => false;

    #region DefaultLabelColumn

    internal const string DEFAULT_LABEL_COLUMN_NAME = nameof(PopoverRowBase) + "_" + nameof(DefaultLabelColumn);

    [CascadingParameter(Name = DEFAULT_LABEL_COLUMN_NAME)]
    public int DefaultLabelColumn { get; set; } = 3;

    #endregion DefaultLabelColumn

    #region LabelColumn

    [Parameter]
    public int? LabelColumn { get; set; }

    #endregion LabelColumn

    #region Title

    [Parameter]
    public string Title { get; set; }

    [Parameter, Obsolete]
    public string Label
    {
        get => Title;
        set => Title = value;
    }

    #endregion Title

    [Parameter]
    public string Description { get; set; }

    [Parameter]
    public bool? IsVisible { get; set; }
}
