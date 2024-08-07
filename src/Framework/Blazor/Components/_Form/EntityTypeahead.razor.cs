﻿using Shipwreck.BlazorTypeahead;

namespace Shipwreck.ViewModelUtils.Components;

public partial class EntityTypeahead : BindableComponentBase<IEntitySelector>, IDisposable
{
    private TypeaheadProxy<object> _Proxy;

    [Inject]
    public IJSRuntime JS { get; set; }

    [Parameter]
    public int MaxCount { get; set; } = 8;

    [Parameter]
    public int MinLength { get; set; } = 1;

    [Parameter]
    public bool? IsEnabled { get; set; }

    [Parameter]
    public string AppendToSelector { get; set; } = ".body-root";

    protected override void OnDataContextSet(IEntitySelector dataContext)
    {
        base.OnDataContextSet(dataContext);
        _Text = dataContext?.SelectedItem == null ? null : dataContext.GetCode(dataContext.SelectedItem);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var t = base.OnAfterRenderAsync(firstRender);
        if (t != null)
        {
            await t;
        }

        if (firstRender)
        {
            _Proxy = await JS.TypeaheadAsync(_Input, new TypeaheadOptions<object>
            {
                AppendToSelector = AppendToSelector,
                Items = MaxCount,
                Delay = 250,
                MinLength = MinLength,
                SourceCallback = async (query, start, end) =>
                {
                    if (DataContext != null)
                    {
                        var r = await DataContext.SearchAsync("^=" + query, MaxCount);
                        return r.Cast<object>().ToList();
                    }
                    return Array.Empty<object>();
                },
                DisplayText = item => DataContext?.GetDisplayText(item),
                AfterSelect = item =>
                {
                    DataContext.SelectedItem = item;
                    _Text = DataContext?.GetCode(DataContext.SelectedItem);
                    StateHasChanged();
                }
            });
        }
    }

    public ValueTask FocusAsync(bool selectAll = false)
        => JS.FocusAsync(_Input, selectAll);

    public void Dispose()
    {
        _Proxy?.DestroyAsync();
        _Proxy = null;
    }

    private ElementReference _Input;
    private string _Text;

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes { get; set; }

    protected override bool OnDataContextPropertyChanged(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(DataContext.SelectedId):
                if (DataContext?.SelectedId == null)
                {
                    _Text = null;
                    return true;
                }
                break;

            case nameof(DataContext.SelectedItem):
                if (DataContext?.SelectedItem != null)
                {
                    _Text = DataContext.GetCode(DataContext.SelectedItem);
                    return true;
                }
                break;
        }
        return false;
    }
}
