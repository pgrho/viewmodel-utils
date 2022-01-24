namespace Shipwreck.ViewModelUtils.Components;

public abstract class PopoverRowBase : BindableComponentBase
{
    protected override bool ImplicitRender => false;

    #region DefaultLabelColumn

    internal const string DEFAULT_LABEL_COLUMN_NAME = nameof(PopoverRowBase) + "_" + nameof(DefaultLabelColumn);

    private int _DefaultLabelColumn = 3;


    [CascadingParameter(Name = DEFAULT_LABEL_COLUMN_NAME)]
    public int DefaultLabelColumn
    {
        get => _DefaultLabelColumn;
        set => SetProperty(ref _DefaultLabelColumn, value);
    }

    #endregion DefaultLabelColumn

    #region LabelColumn

    private int? _LabelColumn;

    [Parameter]
    public int? LabelColumn
    {
        get => _LabelColumn;
        set => SetProperty(ref _LabelColumn, value);
    }

    #endregion LabelColumn

    #region Title

    private string _Title;

    [Parameter]
    public string Title
    {
        get => _Title;
        set => SetProperty(ref _Title, value);
    }

    [Parameter, Obsolete]
    public string Label
    {
        get => Title;
        set => Title = value;
    }

    #endregion Title

    #region Description

    private string _Description;

    [Parameter]
    public string Description
    {
        get => _Description;
        set => SetProperty(ref _Description, value);
    }

    #endregion Description

    #region IsVisible

    private bool? _IsVisible;

    [Parameter]
    public bool? IsVisible
    {
        get => _IsVisible;
        set => SetProperty(ref _IsVisible, value);
    }

    #endregion IsVisible
}
