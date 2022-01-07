namespace Shipwreck.ViewModelUtils;

public sealed class ExtensionColumnCommandViewModel : SelectionCommandViewModelBase
{
    private readonly IHasExtensionColumns _Page;
    public ExtensionColumnCommandViewModel(IHasExtensionColumns page, string value)
        : base(title: value, isSelected: page.SelectedExtensionColumns.Contains(value))
    {
        Value = value;
        _Page = page;
        page.PropertyChanged += Page_PropertyChanged;
        Invalidate();
    }

    public string Value { get; }

    public new string Title
    {
        get => base.Title;
        internal set => base.Title = value;
    }

    public override void Execute()
    {
        if (IsEnabled)
        {
            IsExecuting = true;

            if (IsSelected)
            {
                IsSelected = false;
                _Page.SelectedExtensionColumns = _Page.ExtensionColumns.Where(e => e != Value && _Page.SelectedExtensionColumns.Contains(e)).ToList();
            }
            else
            {
                IsSelected = true;
                _Page.SelectedExtensionColumns = _Page.ExtensionColumns.Where(e => e == Value || _Page.SelectedExtensionColumns.Contains(e)).ToList();
            }

            IsExecuting = false;
        }
    }

    private void Page_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!IsExecuting && e.PropertyName == nameof(IHasExtensionColumns.SelectedExtensionColumns))
        {
            IsSelected = _Page.SelectedExtensionColumns.Contains(Value);
        }
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _Page.PropertyChanged -= Page_PropertyChanged;
        }
    }
}
