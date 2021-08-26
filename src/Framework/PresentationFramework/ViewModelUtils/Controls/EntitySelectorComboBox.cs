using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Shipwreck.ViewModelUtils.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    public class EntitySelectorComboBox : Control
    {
        private class Item
        {
            public Item(object entity, string code, string name, string displayName)
            {
                Entity = entity;
                Code = code;
                Name = name;
                DisplayName = displayName;
            }

            public object Entity { get; }

            public string Code { get; }
            public string Name { get; }
            public string DisplayName { get; }

            public override string ToString() => DisplayName;
        }

        static EntitySelectorComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EntitySelectorComboBox), new FrameworkPropertyMetadata(typeof(EntitySelectorComboBox)));
        }

        public EntitySelectorComboBox()
        {
            _Items = new BulkUpdateableCollection<Item>();
            DataContextChanged += EntitySelectorComboBox_DataContextChanged;
        }

        private void EntitySelectorComboBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is IEntitySelector os)
            {
                os.PropertyChanged -= Selector_PropertyChanged;
                os.RequestFocus -= Selector_RequestFocus;
            }
            if (e.NewValue is IEntitySelector ns)
            {
                ns.PropertyChanged -= Selector_PropertyChanged;
                ns.PropertyChanged += Selector_PropertyChanged;

                ns.RequestFocus -= Selector_RequestFocus;
                ns.RequestFocus += Selector_RequestFocus;
            }
        }

        private void Selector_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Selector.SelectedItem) && Selector != null)
            {
                var entity = Selector.SelectedItem;
                Text = entity != null ? Selector.GetCode(entity) : string.Empty;
            }
        }

        private void Selector_RequestFocus(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            => Focus(true);

        private readonly BulkUpdateableCollection<Item> _Items;
        private string _ItemsQuery;

        private const int MaxItems = 16;

        #region TextBoxStyle

        public static readonly DependencyProperty TextBoxStyleProperty
            = DependencyProperty.Register(
                nameof(TextBoxStyle),
                typeof(Style),
                typeof(EntitySelectorComboBox),
                new FrameworkPropertyMetadata(null));

        public Style TextBoxStyle
        {
            get => (Style)GetValue(TextBoxStyleProperty);
            set => SetValue(TextBoxStyleProperty, value);
        }

        #endregion TextBoxStyle

        #region ListBoxStyle

        public static readonly DependencyProperty ListBoxStyleProperty
            = DependencyProperty.Register(
                nameof(ListBoxStyle),
                typeof(Style),
                typeof(EntitySelectorComboBox),
                new FrameworkPropertyMetadata(null));

        public Style ListBoxStyle
        {
            get => (Style)GetValue(ListBoxStyleProperty);
            set => SetValue(ListBoxStyleProperty, value);
        }

        #endregion ListBoxStyle

        #region Text

        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register(nameof(Text), typeof(string), typeof(EntitySelectorComboBox), new FrameworkPropertyMetadata(string.Empty)
            {
                BindsTwoWayByDefault = true
            });

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion Text

        #region IsOpen

        private static readonly DependencyPropertyKey IsOpenPropertyKey
            = DependencyProperty.RegisterReadOnly(nameof(IsOpen), typeof(bool), typeof(EntitySelectorComboBox), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsOpenProperty = IsOpenPropertyKey.DependencyProperty;

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            private set => SetValue(IsOpenPropertyKey, value);
        }

        #endregion IsOpen

        private IEntitySelector Selector => DataContext as IEntitySelector;

        #region TextBox

        private TextBox _TextBox;

        private TextBox TextBox
        {
            get => _TextBox;
            set
            {
                if (value != _TextBox)
                {
                    if (_TextBox != null)
                    {
                        TextBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                        TextBox.KeyDown -= TextBox_KeyDown;
                        TextBox.PreviewTextInput -= TextBox_PreviewTextInput;
                        TextBox.LostFocus -= TextBox_LostFocus;
                    }
                    _TextBox = value;

                    if (_TextBox != null)
                    {
                        TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                        TextBox.KeyDown += TextBox_KeyDown;
                        TextBox.PreviewTextInput += TextBox_PreviewTextInput;
                        TextBox.LostFocus += TextBox_LostFocus;
                    }
                }
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Handled)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        if (ListBox != null && IsOpen && _Items.Any())
                        {
                            _IsSelecting = true;
                            ListBox.SelectedIndex = ListBox.SelectedIndex < 0 ? _Items.Count - 1 : Math.Max(0, ListBox.SelectedIndex - 1);
                            _IsSelecting = false;
                            e.Handled = true;
                        }
                        break;

                    case Key.Down:
                        if (ListBox != null && IsOpen && _Items.Any())
                        {
                            _IsSelecting = true;
                            ListBox.SelectedIndex = Math.Min(_Items.Count - 1, ListBox.SelectedIndex + 1);
                            _IsSelecting = false;
                            e.Handled = true;
                        }
                        break;
                }
            }
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!e.Handled)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        if (Selector != null)
                        {
                            var q = Text.TrimOrEmpty();

                            if (string.IsNullOrWhiteSpace(q))
                            {
                                Selector.Select(null);
                            }
                            else if (IsOpen && ListBox?.SelectedItem is Item item)
                            {
                                Selector.Select(item.Entity);
                            }
                            else
                            {
                                var matched = _Items.FirstOrDefault(e => Selector.GetMatchDistance(q, e.Entity) == 0);
                                if (matched != null)
                                {
                                    Selector.Select(matched.Entity);
                                }
                                else
                                {
                                    Selector.SelectByCodeAsync(q, true);
                                }
                            }
                            IsOpen = false;
                            e.Handled = true;
                        }
                        break;

                    case Key.Escape:
                        IsOpen = false;
                        e.Handled = true;
                        break;
                }
            }
        }

        private async void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            if (Selector != null)
            {
                await Task.Delay(1);

                var q = TextBox.Text.Trim();
                if (string.IsNullOrEmpty(q))
                {
                    IsOpen = false;
                    _Items.Clear();
                    _ItemsQuery = null;
                }
                else if (_Items.Any()
                    && !string.IsNullOrEmpty(_ItemsQuery)
                    && q.StartsWith(_ItemsQuery)
                    && _Items.Count < MaxItems)
                {
                    _IsSelecting = true;
                    for (var i = _Items.Count - 1; i >= 0; i--)
                    {
                        if (Selector.GetMatchDistance(q, _Items[i].Entity) > 1)
                        {
                            _Items.RemoveAt(i);
                        }
                    }
                    _IsSelecting = false;
                    _ItemsQuery = q;
                    IsOpen = _Items.Any();
                }
                else
                {
                    var list = (await Selector.SearchAsync("^=" + q, MaxItems))?.Cast<object>().ToList();
                    var cq = TextBox?.Text?.Trim();
                    if (list != null
                        && cq?.StartsWith(q) == true)
                    {
                        _IsSelecting = true;
                        var newItems = list.Select(
                            e => _Items.FirstOrDefault(i => i.Entity == e)
                                ?? new Item(
                                    e,
                                    Selector.GetCode(e),
                                    Selector.GetName(e),
                                    Selector.GetDisplayText(e))).ToList();

                        if (!newItems.SequenceEqual(_Items))
                        {
                            _Items.Set(newItems);
                        }
                        _ItemsQuery = q;
                        _IsSelecting = false;
                        IsOpen = _Items.Any();
                    }
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion TextBox

        #region ListBox

        private bool _IsSelecting;

        private ListBox _ListBox;

        private ListBox ListBox
        {
            get => _ListBox;
            set
            {
                if (value != _ListBox)
                {
                    if (_ListBox != null)
                    {
                        _ListBox.SelectionChanged -= _ListBox_SelectionChanged;
                    }
                    _ListBox = value;

                    if (_ListBox != null)
                    {
                        _ListBox.ItemsSource = _Items;
                        _ListBox.SelectionChanged += _ListBox_SelectionChanged;
                    }
                }
            }
        }

        private void _ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_IsSelecting && Selector != null && ListBox?.SelectedItem is Item item)
            {
                Selector.Select(item.Entity);
                IsOpen = false;
            }
        }

        #endregion ListBox

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TextBox = GetTemplateChild("PART_TextBox") as TextBox;
            ListBox = GetTemplateChild("PART_ListBox") as ListBox;
        }

        public void Focus(bool shouldSelect)
        {
            TextBox?.Focus(shouldSelect: shouldSelect);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            TextBox?.Focus(shouldSelect: true);
            base.OnGotFocus(e);
        }
    }
}
