namespace Shipwreck.ViewModelUtils;

public class FlagColumnCommandViewModel : SelectionCommandViewModelBase
{
    public FlagColumnCommandViewModel(IHasColumns page, long value, string title, bool isSelected = false, long unselectValue = 0)
        : base(title: title, isSelected: isSelected)
    {
        Page = page;
        Value = value;
        UnselectValue = unselectValue;
        Invalidate();
        Page.PropertyChanged += Page_PropertyChanged;
    }

    internal IHasColumns Page { get; }

    internal long Value { get; private protected set; }
    internal long UnselectValue { get; private protected set; }

    public override void Execute()
    {
        if (IsEnabled)
        {
            IsExecuting = true;
            OnExecute();
            IsExecuting = false;
        }
    }

    protected virtual void OnExecute()
    {
        var c = Page.Columns;

        if (IsSelected)
        {
            c = (c & ~Value) | UnselectValue;
            IsSelected = false;
        }
        else
        {
            c = (c | Value) & ~UnselectValue;
            IsSelected = true;
        }
        Page.Columns = c;
    }

    private void Page_PropertyChanged(object sender, PropertyChangedEventArgs e)
        => OnPagePropertyChanged(e);

    protected virtual void OnPagePropertyChanged(PropertyChangedEventArgs e)
    {
        if (!IsExecuting && e.PropertyName == nameof(IHasColumns.Columns))
        {
            OnColumnsChanged();
        }
    }

    protected virtual void OnColumnsChanged()
    {
        IsSelected = (Page.Columns & (Value | UnselectValue)) == Value;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            Page.PropertyChanged -= Page_PropertyChanged;
        }
    }
}
