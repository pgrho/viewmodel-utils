namespace Shipwreck.ViewModelUtils.Components;

public partial class EntitySelectorView
{
    #region ItemsTask

    private Task<System.Collections.IList> _ItemsTask;

    private Task<System.Collections.IList> ItemsTask
    {
        get
        {
            if (_ItemsTask == null && DataContext.UseList)
            {
                _ItemsTask = DataContext.GetItemsTask();
                if (_ItemsTask.Status < TaskStatus.RanToCompletion)
                {
                    _ItemsTask.ContinueWith(_ => StateHasChanged());
                }
                _ItemsTask.ContinueWith(_ =>
                {
                    if (DataContext.SelectedId != null
                        && DataContext.SelectedItem == null
                        && DataContext.GetById(DataContext.SelectedId) is object sel)
                    {
                        DataContext.SelectedItem = sel;
                    }
                });
            }
            return _ItemsTask;
        }
    }

    #endregion ItemsTask

    protected override bool ImplicitRender => false;

    #region AppendToSelector

    private string _AppendToSelector = ".body-root";

    [Parameter]
    public string AppendToSelector
    {
        get => _AppendToSelector;
        set => SetProperty(ref _AppendToSelector, value);
    }

    #endregion AppendToSelector

    #region ElementId

    private static int _NewId;

    public string _ElementId;

    [Parameter]
    public string ElementId
    {
        get => _ElementId ??= ($"entity--selector--view--{++_NewId}");
        set => SetProperty(ref _ElementId, value);
    }

    [Obsolete("Use ElementId")]
    public string Id => ElementId;

    #endregion ElementId

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

    #region NameStyle

    private string _NameStyle;

    [Parameter]
    public string NameStyle
    {
        get => _NameStyle;
        set => SetProperty(ref _NameStyle, value);
    }

    #endregion NameStyle

    #region IsNameVisible

    private bool _IsNameVisible = true;

    [Parameter]
    public bool IsNameVisible
    {
        get => _IsNameVisible;
        set => SetProperty(ref _IsNameVisible, value);
    }

    #endregion IsNameVisible

    private string SelectedCode
    {
        get => DataContext?.SelectedItem != null ? DataContext.GetCode(DataContext.SelectedItem) : null;
        set => SetSelectedCode(value);
    }

    private async void SetSelectedCode(string value)
    {
        try
        {
            _IsUpdatingSelectedCode = true;
            if (string.IsNullOrEmpty(value))
            {
                DataContext.SelectedItem = null;
            }
            else if (DataContext.UseList && ItemsTask is Task<System.Collections.IList> t)
            {
                var items = await t;
                DataContext.SelectedItem = items.OfType<object>().FirstOrDefault(e => DataContext.GetCode(e) == value)
                                        ?? DataContext.SelectedItem;
            }
        }
        finally
        {
            _IsUpdatingSelectedCode = false;
        }
    }

    [Parameter]
    public ICommand EnterCommand { get; set; }

    [Parameter]
    public object EnterCommandParameter { get; set; }

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

    private void OnSelectKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            EnterCommand?.Execute(EnterCommandParameter);
        }
    }
}
