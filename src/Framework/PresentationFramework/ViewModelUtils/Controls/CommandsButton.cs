using System.Windows;
using System.Windows.Controls;

namespace Shipwreck.ViewModelUtils.Controls
{
    public class CommandsButton : Control
    {
        static CommandsButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandsButton), new FrameworkPropertyMetadata(typeof(CommandsButton)));
        }

        public static readonly DependencyProperty DropDownIconProperty
            = DependencyProperty.Register(
                nameof(DropDownIcon),
                typeof(string),
                typeof(CommandsButton),
                new FrameworkPropertyMetadata(null));

        public string DropDownIcon
        {
            get => (string)GetValue(DropDownIconProperty);
            set => SetValue(DropDownIconProperty, value);
        }

        public static readonly DependencyProperty DropDownTitleProperty
            = DependencyProperty.Register(
                nameof(DropDownTitle),
                typeof(string),
                typeof(CommandsButton),
                new FrameworkPropertyMetadata(null));

        public string DropDownTitle
        {
            get => (string)GetValue(DropDownTitleProperty);
            set => SetValue(DropDownTitleProperty, value);
        }

        public static readonly DependencyProperty DropDownStyleProperty
            = DependencyProperty.Register(
                nameof(DropDownStyle),
                typeof(BorderStyle),
                typeof(CommandsButton),
                new FrameworkPropertyMetadata(BorderStyle.OutlineSecondary));

        public BorderStyle DropDownStyle
        {
            get => (BorderStyle)GetValue(DropDownStyleProperty);
            set => SetValue(DropDownStyleProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty
            = DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(CommandViewModelCollection),
                typeof(CommandsButton),
                new FrameworkPropertyMetadata(null));

        public CommandViewModelCollection ItemsSource
        {
            get => (CommandViewModelCollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        #region IsDropDown

        public static readonly DependencyProperty IsDropDownProperty
            = DependencyProperty.Register(nameof(IsDropDown), typeof(bool), typeof(CommandsButton), new PropertyMetadata(false));

        public bool IsDropDown
        {
            get => (bool)GetValue(IsDropDownProperty);
            set => SetValue(IsDropDownProperty, value);
        }

        #endregion IsDropDown
    }
}
