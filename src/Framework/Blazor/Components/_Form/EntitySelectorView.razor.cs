namespace Shipwreck.ViewModelUtils.Components;

public partial class EntitySelectorView
{
    private Task<System.Collections.IList> _ItemsTask;

    #region AppendToSelector

    private string _AppendToSelector = ".body-root";

    [Parameter]
    public string AppendToSelector
    {
        get => _AppendToSelector;
        set => SetProperty(ref _AppendToSelector, value);
    }

    #endregion AppendToSelector

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

    #region CodeClass

    private string _CodeClass = "entity-selector-code";

    [Parameter]
    public string CodeClass
    {
        get => _CodeClass;
        set => SetProperty(ref _CodeClass, value);
    }

    #endregion CodeClass

    #region CodeStyle

    private string _CodeStyle = "border-top-right-radius:0;border-bottom-right-radius:0;";

    [Parameter]
    public string CodeStyle
    {
        get => _CodeStyle;
        set => SetProperty(ref _CodeStyle, value);
    }

    #endregion CodeStyle

    #region NameClass

    private string _NameClass = "entity-selector-name";

    [Parameter]
    public string NameClass
    {
        get => _NameClass;
        set => SetProperty(ref _NameClass, value);
    }

    #endregion NameClass

    #region NameeStyle

    private string _NameeStyle;

    [Parameter]
    public string NameeStyle
    {
        get => _NameeStyle;
        set => SetProperty(ref _NameeStyle, value);
    }

    #endregion NameeStyle

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
