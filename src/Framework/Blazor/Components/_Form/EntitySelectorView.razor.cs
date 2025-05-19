namespace Shipwreck.ViewModelUtils.Components;

using KeyboardEventArgs = Microsoft.AspNetCore.Components.Web.KeyboardEventArgs;

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
                    _ItemsTask.ContinueWith(_ => StateHasChanged()
#if IS_WEBVIEW
            , TaskScheduler.FromCurrentSynchronizationContext()
#endif
                    );
                }
                _ItemsTask.ContinueWith(_ =>
                {
                    if (DataContext.SelectedId != null
                        && DataContext.SelectedItem == null
                        && DataContext.GetById(DataContext.SelectedId) is object sel)
                    {
                        DataContext.SelectedItem = sel;
                    }
                }
#if IS_WEBVIEW
            , TaskScheduler.FromCurrentSynchronizationContext()
#endif
            );
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

    [CascadingParameter]
    public FormGroupTheme Theme { get; set; }

    [Parameter]
    public bool? IsEnabled { get; set; }

    [Parameter]
    public bool IsRequired { get; set; }

    [Parameter]
    public bool? UseList { get; set; }

    [Parameter]
    public string GroupClass { get; set; } = "entity-selector";

    [Parameter]
    public string CodeClass { get; set; } = "entity-selector-code";

    [Parameter]
    public string CodeStyle { get; set; } = "border-top-right-radius:0;border-bottom-right-radius:0;";

    [Parameter]
    public string NameClass { get; set; } = "entity-selector-name";

    [Parameter]
    public string NameStyle { get; set; }

    [Parameter]
    public bool IsNameVisible { get; set; } = true;

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
