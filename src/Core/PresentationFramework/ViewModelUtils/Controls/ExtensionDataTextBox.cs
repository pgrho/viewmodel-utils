using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shipwreck.ViewModelUtils.Controls
{
    public class ExtensionDataTextBox : TextBox
    {
        private bool _IsUpdating;

        #region ExtensionData

        public static readonly DependencyProperty ExtensionDataProperty
            = DependencyProperty.Register(
                nameof(ExtensionData),
                typeof(IEnumerable<KeyValuePair<string, string>>), typeof(ExtensionDataTextBox),
                new FrameworkPropertyMetadata(null, (d, e) => (d as ExtensionDataTextBox)?.OnExtensionDataChanged(e)));

        public IEnumerable<KeyValuePair<string, string>> ExtensionData
        {
            get => (IEnumerable<KeyValuePair<string, string>>)GetValue(ExtensionDataProperty);
            set => SetValue(ExtensionDataProperty, value);
        }

        private void OnExtensionDataChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged old)
            {
                old.CollectionChanged -= ExtensionData_CollectionChanged;
            }
            if (e.NewValue is INotifyCollectionChanged n)
            {
                n.CollectionChanged += ExtensionData_CollectionChanged;
            }
            OnExtensionDataKeyChanged();
        }

        private void ExtensionData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_IsUpdating && ExtensionDataKey != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems?.Cast<KeyValuePair<string, string>>().Any(e => e.Key == ExtensionDataKey) == false)
                        {
                            return;
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems?.Cast<KeyValuePair<string, string>>().Any(e => e.Key == ExtensionDataKey) == false)
                        {
                            return;
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        if (e.NewItems?.Cast<KeyValuePair<string, string>>().Any(e => e.Key == ExtensionDataKey) == false
                            && e.OldItems?.Cast<KeyValuePair<string, string>>().Any(e => e.Key == ExtensionDataKey) == false)
                        {
                            return;
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        return;
                }
                OnExtensionDataKeyChanged();
            }
        }

        #endregion ExtensionData

        #region ExtensionDataKey

        public static readonly DependencyProperty ExtensionDataKeyProperty
            = DependencyProperty.Register(
                nameof(ExtensionDataKey),
                typeof(string),
                typeof(ExtensionDataTextBox), new FrameworkPropertyMetadata(null, (d, e) => (d as ExtensionDataTextBox)?.OnExtensionDataKeyChanged()));

        public string ExtensionDataKey
        {
            get => (string)GetValue(ExtensionDataKeyProperty);
            set => SetValue(ExtensionDataKeyProperty, value);
        }

        private void OnExtensionDataKeyChanged()
        {
            Text = ExtensionDataKey != null ? ExtensionData?.FirstOrDefault(e => e.Key == ExtensionDataKey).Value : null;
        }

        #endregion ExtensionDataKey

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (ExtensionDataKey != null
                && ExtensionData is IList<KeyValuePair<string, string>> l)
            {
                try
                {
                    _IsUpdating = true;

                    if (string.IsNullOrWhiteSpace(Text))
                    {
                        for (var i = l.Count - 1; i >= 0; i--)
                        {
                            if (l[i].Key == ExtensionDataKey)
                            {
                                l.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        var v = Text.Trim();
                        if (l.Any(e => e.Key == ExtensionDataKey && e.Value == v))
                        {
                            return;
                        }

                        var added = false;

                        for (var i = l.Count - 1; i >= 0; i--)
                        {
                            if (l[i].Key == ExtensionDataKey)
                            {
                                if (added)
                                {
                                    l.RemoveAt(i);
                                }
                                else
                                {
                                    l[i] = new KeyValuePair<string, string>(ExtensionDataKey, v);
                                    added = true;
                                }
                            }
                        }

                        if (!added)
                        {
                            l.Add(new KeyValuePair<string, string>(ExtensionDataKey, v));
                        }
                    }
                }
                finally
                {
                    _IsUpdating = false;
                }
            }
        }
    }
}
