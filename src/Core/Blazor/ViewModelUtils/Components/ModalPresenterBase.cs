﻿namespace Shipwreck.ViewModelUtils.Components;

public abstract class ModalPresenterBase : BindableComponentBase
{
    protected Type ModalType { get; private set; }

    public KeyValuePair<string, object>[] _Properties;

    private WeakReference<ComponentBase> _Modal;

    public ComponentBase Modal
    {
        get => _Modal != null && _Modal.TryGetTarget(out var c) ? c : null;
        private set
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
                    newModal.Presenter = this;
                }
            }
        }
    }

    public void ShowModal(Type modalType, IEnumerable<KeyValuePair<string, object>> properties)
    {
        ModalType = modalType;
        _Properties = properties?.ToArray();

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
    {
        var m = Modal;
        if (m != null)
        {
            ShowModal(null, null);
            (m as IDisposable)?.Dispose();
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (ModalType != null)
        {
            var isModal = typeof(IModal).IsAssignableFrom(ModalType);
            var i = 0;

            builder.OpenComponent(i++, ModalType);

            if (_Properties?.Length > 0)
            {
                foreach (var kv in _Properties)
                {
                    builder.AddAttribute(i++, kv.Key, kv.Value);
                }
            }

            builder.AddComponentReferenceCapture(i++, m => Modal = m as ComponentBase);

            builder.CloseComponent();

            ModalType = null;
            _Properties = null;

            if (isModal)
            {
                BuildModalBackdropTree(builder, i);
            }
        }
    }

    protected abstract void BuildModalBackdropTree(RenderTreeBuilder builder, int sequence);
}
