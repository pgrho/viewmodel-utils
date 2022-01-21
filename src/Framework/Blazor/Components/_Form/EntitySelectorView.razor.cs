namespace Shipwreck.ViewModelUtils.Components;

public partial class EntitySelectorView
{
    private Task<System.Collections.IList> _ItemsTask;

    [Parameter]
    public string AppendToSelector { get; set; } = ".body-root";

    #region Id

    private static int _NewId;

    public string _Id;

    public string Id => _Id ??= ($"entity--selector--view--{++_NewId}");

    #endregion Id

    #region Theme

    private FormGroupTheme _Theme;

    [CascadingParameter]
    public FormGroupTheme Theme
    {
        get => _Theme;
        set => SetProperty(ref _Theme, value);
    }

    #endregion Theme

    #region IsEnabled

    private bool? _IsEnabled;

    [Parameter]
    public bool? IsEnabled
    {
        get => _IsEnabled;
        set => SetProperty(ref _IsEnabled, value);
    }

    #endregion IsEnabled

    #region IsRequired

    private bool _IsRequired;

    [Parameter]
    public bool IsRequired
    {
        get => _IsRequired;
        set => SetProperty(ref _IsRequired, value);
    }

    #endregion IsRequired

    [Parameter]
    public string CodeClass { get; set; } = "entity-selector-code";

    [Parameter]
    public string CodeStyle { get; set; } = "border-top-right-radius:0;border-bottom-right-radius:0;";

    [Parameter]
    public string NameClass { get; set; } = "entity-selector-name";

    [Parameter]
    public string NameeStyle { get; set; }

    private string SelectedCode
    {
        get => DataContext?.SelectedItem != null ? DataContext.GetCode(DataContext.SelectedItem) : null;
        set
        {
            try
            {
                _IsUpdatingSelectedCode = true;
                if (string.IsNullOrEmpty(value))
                {
                    DataContext.SelectedItem = null;
                }
                else if (_ItemsTask?.Status == TaskStatus.RanToCompletion)
                {
                    DataContext.SelectedItem = _ItemsTask.Result.OfType<object>().FirstOrDefault(e => DataContext.GetCode(e) == value)
                                            ?? DataContext.SelectedItem;
                }
            }
            finally
            {
                _IsUpdatingSelectedCode = false;
            }
        }
    }

    private bool _IsUpdatingSelectedCode;

    protected override bool OnDataContextPropertyChanged(string propertyName)
    {
        _ItemsTask = null;
        if (_IsUpdatingSelectedCode)
        {
            switch (propertyName)
            {
                case nameof(DataContext.SelectedId):
                case nameof(DataContext.SelectedItem):
                    return false;
            }
        }
        return base.OnDataContextPropertyChanged(propertyName);
    }
}
