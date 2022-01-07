using System.Collections.Specialized;

namespace Shipwreck.ViewModelUtils.Controls;

public class ExtensionDataTextBlock : TextBlock
{
    #region ExtensionData

    public static readonly DependencyProperty ExtensionDataProperty
        = DependencyProperty.Register(
            nameof(ExtensionData),
            typeof(IEnumerable<KeyValuePair<string, string>>), typeof(ExtensionDataTextBlock),
            new FrameworkPropertyMetadata(null, (d, e) => (d as ExtensionDataTextBlock)?.OnExtensionDataChanged(e)));

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
        if (ExtensionDataKey != null)
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

    #region DataKey

    public static readonly DependencyProperty ExtensionDataKeyProperty
        = DependencyProperty.Register(
            nameof(ExtensionDataKey),
            typeof(string),
            typeof(ExtensionDataTextBlock), new FrameworkPropertyMetadata(null, (d, e) => (d as ExtensionDataTextBlock)?.OnExtensionDataKeyChanged()));

    public string ExtensionDataKey
    {
        get => (string)GetValue(ExtensionDataKeyProperty);
        set => SetValue(ExtensionDataKeyProperty, value);
    }

    private void OnExtensionDataKeyChanged()
    {
        Text = ExtensionDataKey != null ? ExtensionData?.FirstOrDefault(e => e.Key == ExtensionDataKey).Value : null;
    }

    #endregion DataKey
}
