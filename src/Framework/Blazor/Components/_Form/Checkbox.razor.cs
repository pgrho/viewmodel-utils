namespace Shipwreck.ViewModelUtils.Components;

public partial class Checkbox : ComponentBase
{
    public static Func<int, string> IdCreator { get; set; } = i => $"checkbox-{i}";

    private static int _NewId;
    private readonly string _Id = IdCreator(++_NewId);

    [Inject]
    public IJSRuntime JS { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    private bool? _IsChecked;
    private bool _ScriptIndeterminate;

    [Parameter]
    public bool IsChecked
    {
        get => _IsChecked ?? false;
        set => NullableIsChecked = value;
    }

    [Parameter]
    public bool? NullableIsChecked
    {
        get => _IsChecked;
        set
        {
            if (_IsChecked != value)
            {
                _IsChecked = value;
                NullableIsCheckedChanged?.Invoke(_IsChecked);
                if (_IsChecked != null)
                {
                    IsCheckedChanged?.Invoke(_IsChecked.Value);
                }
                StateHasChanged();

                var indeterminate = (_IsChecked == null);

                if (_ScriptIndeterminate != indeterminate)
                {
                    _ScriptIndeterminate = indeterminate;
                    JS.InvokeVoidAsync("Shipwreck.ViewModelUtils.setIndeterminate", _Id, indeterminate);
                }
            }
        }
    }

    [Parameter]
    public Action<bool> IsCheckedChanged { get; set; }

    [Parameter]
    public Action<bool?> NullableIsCheckedChanged { get; set; }

    private bool _IsEnabled = true;

    [Parameter]
    public bool IsEnabled
    {
        get => _IsEnabled;
        set
        {
            if (_IsEnabled != value)
            {
                _IsEnabled = value;
                StateHasChanged();
            }
        }
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes { get; set; }
}
