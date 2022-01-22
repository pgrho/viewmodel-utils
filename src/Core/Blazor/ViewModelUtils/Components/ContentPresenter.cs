namespace Shipwreck.ViewModelUtils.Components;

public sealed class ContentPresenter : BindableComponentBase
{
    #region ViewType

    private Type _ViewType;

    [Parameter]
    public Type ViewType
    {
        get => _ViewType;
        set => SetProperty(ref _ViewType, value);
    }

    #endregion ViewType

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> Attributes
    {
        get => _Attributes;
        set => SetProperty(ref _Attributes, value);
    }
    private IDictionary<string, object> _Attributes;

    private WeakReference<ComponentBase> _View;

    public ComponentBase View
    {
        get
        {
            if (_View != null && _View.TryGetTarget(out var c))
            {
                if (c is ContentPresenter cp && ViewType != typeof(ContentPresenter))
                {
                    return cp.View;
                }
                return c;
            }
            return null;
        }
        internal set => _View = value == null ? null : new WeakReference<ComponentBase>(value);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (ViewType != null)
        {
            var i = 0;
            builder.OpenComponent(++i, ViewType);

            if (Attributes != null)
            {
                foreach (var kv in Attributes)
                {
                    builder.AddAttribute(++i, kv.Key, kv.Value);
                }
            }
            builder.AddComponentReferenceCapture(++i, c => View = c as ComponentBase);

            builder.CloseComponent();
        }
        else
        {
            _View = null;
        }
    }
}
