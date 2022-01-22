namespace Shipwreck.ViewModelUtils.Components;

public partial class DropdownButton : ListComponentBase<ICommandViewModel>
{
    private static int _NewId;
    private string _Id = "DropdownButton--" + (++_NewId);

    #region BaseClass

    private string _BaseClass = "btn";

    [Parameter]
    public string BaseClass
    {
        get => _BaseClass;
        set => SetProperty(ref _BaseClass, value);
    }

    #endregion BaseClass

    [Parameter]
    [Obsolete]
    public BorderStyle? Type
    {
        get => Style;
        set => Style = value;
    }

    #region Style

    private BorderStyle? _Style;

    [Parameter]
    public BorderStyle? Style
    {
        get => _Style;
        set => SetProperty(ref _Style, value);
    }

    #endregion Style

    #region ChildContent

    private RenderFragment _ChildContent;

    [Parameter]
    public RenderFragment ChildContent
    {
        get => _ChildContent;
        set => SetProperty(ref _ChildContent, value);
    }

    #endregion ChildContent

    #region Description

    private string _Description;

    [Parameter]
    public string Description
    {
        get => _Description;
        set => SetProperty(ref _Description, value);
    }

    #endregion Description
}
