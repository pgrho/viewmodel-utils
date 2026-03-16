namespace Shipwreck.ViewModelUtils.Components;

public abstract class ModalPresenterBase : BindableComponentBase
{
    public sealed class ModalStack
    {
        public ModalStack(ModalPresenterBase presenter, Type modalType, IEnumerable<KeyValuePair<string, object>> properties)
        {
            Presenter = presenter;
            ModalType = modalType;
            Properties = new(properties?.ToArray() ?? []);
        }

        public ModalPresenterBase Presenter { get; }

        public Type ModalType { get; set; }
        public ReadOnlyCollection<KeyValuePair<string, object>> Properties { get; }

        private WeakReference<ComponentBase> _Modal;

        public ComponentBase Modal
        {
            get => _Modal != null && _Modal.TryGetTarget(out var c) ? c : null;
            internal set
            {
                var current = Modal;
                if (current != value)
                {
                    _Modal = value == null ? null : new WeakReference<ComponentBase>(value);
                    if (current is IModal currentModal)
                    {
                        currentModal.Presenter = null;
                    }
                    if (value is IModal newModal)
                    {
                        newModal.Presenter = Presenter;
                    }
                }
            }
        }

        public bool IsRendered { get; set; }
    }

    private readonly List<ModalStack> _Stack = new();

    public bool HasStack => _Stack.Any();

    public void ShowModal(Type modalType, IEnumerable<KeyValuePair<string, object>> properties)
    {
        _Stack.Add(new(this, modalType, properties));

        StateHasChanged();
    }

    public void ShowModal<TModal, TDataContext>(TDataContext dataContext, bool? isOpen = null)
        where TModal : ModalBase<TDataContext>
        where TDataContext : class
    {
        var props = new List<KeyValuePair<string, object>>(2);
        if (dataContext != null)
        {
            props.Add(new KeyValuePair<string, object>(nameof(ModalBase<TDataContext>.DataContext), dataContext));
        }
        if (isOpen != null)
        {
            props.Add(new KeyValuePair<string, object>(nameof(ModalBase<TDataContext>.IsOpen), isOpen));
        }

        ShowModal(typeof(TModal), props);
    }

    public void CloseModal()
        => CloseModal((e, i) => i == 0);

    public void CloseAll()
        => CloseModal((_, _) => true);

    public void CloseByDataContext(object dataContext)
        => CloseModal(e => e.Properties.FirstOrDefault(p => p.Key == "DataContext").Value == dataContext);

    public void CloseModal(Func<ModalStack, bool> predicate)
        => CloseModal((e, i) => predicate(e));

    public void CloseModal(Func<ModalStack, int, bool> predicate)
    {
        var targets = _Stack.AsEnumerable().Reverse().Where(predicate).ToList();
        var ms = targets.Select(e => e.Modal).ToList();

        if (_Stack.RemoveAll(e => targets.Contains(e)) > 0)
        {
            StateHasChanged();
        }

        foreach (var m in ms)
        {
            (m as IDisposable)?.Dispose();
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var en = _Stack.LastOrDefault();
        if (en != null)
        {
            var i = 0;

            var isModal = typeof(IModal).IsAssignableFrom(en.ModalType);
            //if (!en.IsRendered)
            {
                builder.OpenComponent(i++, en.ModalType);

                foreach (var kv in en.Properties)
                {
                    builder.AddAttribute(i++, kv.Key, kv.Value);
                }

                builder.AddComponentReferenceCapture(i++, m => en.Modal = m as ComponentBase);

                builder.CloseComponent();

                en.IsRendered = true;
            }
            foreach (var pre in _Stack)
            {
                if (pre != en)
                {
                    en.IsRendered = false;
                    en.Modal = null;
                }
            }

            if (isModal)
            {
                BuildModalBackdropTree(builder, i);
            }
        }
    }

    protected abstract void BuildModalBackdropTree(RenderTreeBuilder builder, int sequence);
}
