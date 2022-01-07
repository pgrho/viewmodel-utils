using TypeConverterAttribute = Xamarin.Forms.TypeConverterAttribute;

namespace Shipwreck.ViewModelUtils;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EntitySelectorView
{
    public static readonly BindableProperty FontSizeProperty
               = BindableProperty.Create(
                   nameof(FontSize), typeof(double), typeof(EntitySelectorView),
                   defaultValue: new FontSizeConverter().ConvertFromInvariantString("Medium"));

    public static readonly BindableProperty KeyboardProperty
        = BindableProperty.Create(
            nameof(Keyboard), typeof(Keyboard), typeof(EntitySelectorView),
            defaultValue: new KeyboardTypeConverter().ConvertFromInvariantString("Default"));

    public static readonly BindableProperty TextColorProperty
        = BindableProperty.Create(
            nameof(TextColor), typeof(Color), typeof(EntitySelectorView),
            defaultValue: Color.Default);

    public static readonly BindableProperty PlaceholderColorProperty
        = BindableProperty.Create(
            nameof(PlaceholderColor), typeof(Color), typeof(EntitySelectorView),
            defaultValue: Color.Default);

    public EntitySelectorView()
    {
        InitializeComponent();
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    [TypeConverter(typeof(KeyboardTypeConverter))]
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set => SetValue(PlaceholderColorProperty, value);
    }

    #region ShowListCommand

    private CommandViewModelBase _ShowListCommand;
    public CommandViewModelBase ShowListCommand => _ShowListCommand ??= CreateShowListCommand();

    private sealed class ItemDisplayTextConverter : IValueConverter
    {
        private readonly IEntitySelector _Selector;

        public ItemDisplayTextConverter(IEntitySelector s)
        {
            _Selector = s;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => _Selector.GetDisplayText(value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    private CommandViewModelBase CreateShowListCommand()
        => CommandViewModel.CreateAsync(async () =>
        {
            if (BindingContext is IEntitySelector s && s.UseList)
            {
                try
                {
                    if (picker.ItemsSource == null)
                    {
                        var items = await s.GetItemsTask();

                        picker.ItemDisplayBinding = new Binding
                        {
                            Converter = new ItemDisplayTextConverter(s)
                        };
                        picker.ItemsSource = items;
                    }
                    picker.SelectedIndexChanged -= Picker_SelectedIndexChanged;
                    picker.SelectedItem = s.SelectedItem;
                    picker.SelectedIndexChanged += Picker_SelectedIndexChanged;

                    picker.Focus();
                }
                catch { }
            }
        },
            icon: "fas fa-ellipsis-h",
            isVisibleGetter: () => (BindingContext as IEntitySelector)?.UseList == true);

    private void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (BindingContext is IEntitySelector s
            && s.UseList
            && picker.SelectedItem != null)
        {
            s.SelectedItem = picker.SelectedItem;
        }
    }

    #endregion ShowListCommand

    protected override void OnFocusRequested(string propertyName)
        => entry.Focus();

    public new void Focus()
        => entry?.Focus();

    protected override void OnBindingContextChanged(object bindingContext, object previousBindingContext)
    {
        base.OnBindingContextChanged(bindingContext, previousBindingContext);

        if (previousBindingContext is IEntitySelector p)
        {
            p.PropertyChanged -= C_PropertyChanged;
        }
        if (bindingContext is IEntitySelector c)
        {
            c.PropertyChanged += C_PropertyChanged;
        }
        OnSelectedItemChanged();
        _ShowListCommand?.Invalidate();
    }

    private void C_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IEntitySelector.SelectedItem):
                OnSelectedItemChanged(true);
                break;
        }
    }

    private void OnSelectedItemChanged(bool updateCode = false)
    {
        if (dummyEntry != null)
        {
            var s = BindingContext as IEntitySelector;
            dummyEntry.Placeholder = s?.SelectedItem != null ? s.GetDisplayText(s.SelectedItem) : null;
            if (updateCode || s?.SelectedItem != null && string.IsNullOrEmpty(entry.Text))
            {
                s.Code = s?.SelectedItem == null ? string.Empty : s.GetCode(s.SelectedItem);
            }

            if (!string.IsNullOrEmpty(dummyEntry?.Placeholder)
                && dummyEntry.Placeholder?.StartsWith(entry.Text ?? string.Empty) == true
                && !entry.IsFocused)
            {
                entry.Opacity = 0;
                dummyEntry.Opacity = 1;
            }
            else
            {
                entry.Opacity = 1;
                dummyEntry.Opacity = 0;
            }
        }
    }

    private void entry_Focused(object sender, FocusEventArgs e)
    {
        entry.Opacity = 1;
        dummyEntry.Opacity = 0;

        Dispatcher.BeginInvokeOnMainThread(() =>
        {
            entry.CursorPosition = 0;
            entry.SelectionLength = entry.Text?.Length ?? 0;
        });
    }

    private void entry_Unfocused(object sender, FocusEventArgs e)
    {
        OnSelectedItemChanged();
    }
}
