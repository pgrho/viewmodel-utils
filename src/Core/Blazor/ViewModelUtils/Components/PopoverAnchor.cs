namespace Shipwreck.ViewModelUtils.Components;

public abstract class PopoverAnchor<T> : PopoverTarget<T>
    where T : class
{
    #region IsPrimary

    private bool _IsPrimary;

    [Parameter]
    public bool IsPrimary
    {
        get => _IsPrimary;
        set => SetProperty(ref _IsPrimary, value);
    }

    #endregion IsPrimary

    #region TabIndex

    private int? _TabIndex;

    [Parameter]
    public int? TabIndex
    {
        get => _TabIndex;
        set => SetProperty(ref _TabIndex, value);
    }

    #endregion TabIndex

    #region ChildContent

    private RenderFragment _ChildContent;

    [Parameter]
    public RenderFragment ChildContent
    {
        get => _ChildContent;
        set => SetProperty(ref _ChildContent, value);
    }

    #endregion ChildContent

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes
    {
        get => _AdditionalAttributes;
        set => SetProperty(ref _AdditionalAttributes, value);
    }
    private IDictionary<string, object> _AdditionalAttributes;

    protected virtual int? GetTabIndex() => TabIndex;
}
