namespace Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

public sealed class DateTimeTestPageViewModel : PageViewModel
{
    public DateTimeTestPageViewModel(PageBase page)
        : base(page)
    {
        _Value = DateTime.Today;
    }

    #region Value

    private DateTime _Value;

    public DateTime Value
    {
        get => _Value;
        set => SetProperty(ref _Value, value);
    }

    #endregion Value
}
