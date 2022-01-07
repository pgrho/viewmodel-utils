using System.Collections.Specialized;
using System.Windows.Documents;

namespace Shipwreck.ViewModelUtils.Controls;

public class DataGridExtensionDataColumn : DataGridTextColumn
{
    public Type ExtensionDataType { get; set; }

    public BindingBase ExtensionData { get; set; }
    public string ExtensionDataKey { get; set; }

    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        var tb = new ExtensionDataTextBlock() { ExtensionDataKey = ExtensionDataKey };

        if (ExtensionDataType?.IsAssignableFrom(dataItem.GetType()) == true && ExtensionData != null)
        {
            tb.SetBinding(ExtensionDataTextBlock.ExtensionDataProperty, ExtensionData);
        }

        SyncProperties(tb);

        if (ElementStyle != null)
        {
            tb.Style = ElementStyle;
        }
        return tb;
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        var tb = new ExtensionDataTextBox() { ExtensionDataKey = ExtensionDataKey };
        if (ExtensionDataType?.IsAssignableFrom(dataItem.GetType()) == true && ExtensionData != null)
        {
            tb.SetBinding(ExtensionDataTextBox.ExtensionDataProperty, ExtensionData);
        }

        SyncProperties(tb);

        if (EditingElementStyle != null)
        {
            tb.Style = EditingElementStyle;
        }

        return tb;
    }

    private void SyncProperties(FrameworkElement e)
    {
        SyncColumnProperty(this, e, TextElement.FontFamilyProperty, FontFamilyProperty);
        SyncColumnProperty(this, e, TextElement.FontSizeProperty, FontSizeProperty);
        SyncColumnProperty(this, e, TextElement.FontStyleProperty, FontStyleProperty);
        SyncColumnProperty(this, e, TextElement.FontWeightProperty, FontWeightProperty);
        SyncColumnProperty(this, e, TextElement.ForegroundProperty, ForegroundProperty);
    }

    internal static void SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty contentProperty, DependencyProperty columnProperty)
    {
        if (IsDefaultValue(column, columnProperty))
        {
            content.ClearValue(contentProperty);
        }
        else
        {
            content.SetValue(contentProperty, column.GetValue(columnProperty));
        }
    }

    public static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
        => DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;

    public override object OnCopyingCellClipboardContent(object item)
    {
        if (ExtensionData != null
            && ExtensionDataKey != null
            && ExtensionDataType?.IsAssignableFrom(item.GetType()) == true)
        {
            if (ExtensionData.ResolveValue(item) is IEnumerable<KeyValuePair<string, string>> dic)
            {
                var kv = dic.FirstOrDefault(e => e.Key == ExtensionDataKey);
                if (kv.Key != null)
                {
                    return kv.Value;
                }
            }
        }

        return base.OnCopyingCellClipboardContent(item);
    }

    public override void OnPastingCellClipboardContent(object item, object cellContent)
    {
        if (!IsReadOnly
            && ExtensionData != null
            && ExtensionDataKey != null
            && ExtensionDataType?.IsAssignableFrom(item.GetType()) == true)
        {
            if (ExtensionData.ResolveValue(item) is IEnumerable<KeyValuePair<string, string>> dic)
            {
                var s = ((cellContent as string) ?? cellContent?.ToString()).TrimOrNull();

                if (dic is IDictionary<string, string> td)
                {
                    if (s == null)
                    {
                        td.Remove(ExtensionDataKey);
                    }
                    else
                    {
                        td[ExtensionDataKey] = s;
                    }
                }
                else if (dic is ICollection<KeyValuePair<string, string>> col)
                {
                    if (col is IList<KeyValuePair<string, string>> list && s != null)
                    {
                        for (var i = 0; i < list.Count; i++)
                        {
                            if (list[i].Key == ExtensionDataKey)
                            {
                                list[i] = new KeyValuePair<string, string>(ExtensionDataKey, s);

                                for (var j = list.Count - 1; j > i; j--)
                                {
                                    if (list[j].Key == ExtensionDataKey)
                                    {
                                        list.RemoveAt(j);
                                    }
                                }

                                return;
                            }
                        }
                    }

                    foreach (var kv in col.Where(e => e.Key == ExtensionDataKey).ToList())
                    {
                        col.Remove(kv);
                    }

                    if (s != null)
                    {
                        col.Add(new KeyValuePair<string, string>(ExtensionDataKey, s));
                    }
                }
            }
        }
        base.OnPastingCellClipboardContent(item, cellContent);
    }

    #region ExtensionDataType

    public static Type GetExtensionDataType(DataGrid obj)
    {
        return (Type)obj.GetValue(ExtensionDataTypeProperty);
    }

    public static void SetExtensionDataType(DataGrid obj, Type value)
    {
        obj.SetValue(ExtensionDataTypeProperty, value);
    }

    public static readonly DependencyProperty ExtensionDataTypeProperty
        = DependencyProperty.RegisterAttached(
            "ExtensionDataType",
            typeof(Type),
            typeof(DataGridExtensionDataColumn),
            new FrameworkPropertyMetadata(null, (d, e) =>
            {
                if (d is DataGrid dg)
                {
                    GenerateColumns(dg);
                }
            }));

    #endregion ExtensionDataType

    #region ExtensionDataPath

    public static string GetExtensionDataPath(DataGrid obj)
    {
        return (string)obj.GetValue(ExtensionDataPathProperty);
    }

    public static void SetExtensionDataPath(DataGrid obj, string value)
    {
        obj.SetValue(ExtensionDataPathProperty, value);
    }

    public static readonly DependencyProperty ExtensionDataPathProperty
        = DependencyProperty.RegisterAttached(
            "ExtensionDataPath",
            typeof(string),
            typeof(DataGridExtensionDataColumn),
            new FrameworkPropertyMetadata(null, (d, e) =>
            {
                if (d is DataGrid dg)
                {
                    GenerateColumns(dg);
                }
            }));

    #endregion ExtensionDataPath

    #region ExtensionColumns

    public static readonly DependencyProperty ExtensionColumnsProperty
        = DependencyProperty.RegisterAttached(
            "ExtensionColumns",
            typeof(IEnumerable<string>),
            typeof(DataGridExtensionDataColumn),
            new FrameworkPropertyMetadata(null, (d, e) =>
            {
                if (d is DataGrid dg)
                {
                    GenerateColumns(dg);

                    //if (e.OldValue is INotifyCollectionChanged oncc)
                    //{
                    //}
                    if (e.NewValue is INotifyCollectionChanged nncc)
                    {
                        NotifyCollectionChangedEventHandler h = (s, e) => GenerateColumns(dg);
                        RoutedEventHandler uh = null;
                        uh = (s, e) =>
                        {
                            nncc.CollectionChanged -= h;
                            dg.Unloaded -= uh;
                        };
                        nncc.CollectionChanged += h;
                        dg.Unloaded += uh;
                    }
                }
            }));

    public static IEnumerable<string> GetExtensionColumns(DataGrid obj)
        => (IEnumerable<string>)obj.GetValue(ExtensionColumnsProperty);

    public static void SetExtensionColumns(DataGrid obj, IEnumerable<string> value)
        => obj.SetValue(ExtensionColumnsProperty, value);

    #endregion ExtensionColumns

    #region ExtensionReadOnly

    public static bool GetExtensionReadOnly(DataGrid obj)
        => (bool)obj.GetValue(ExtensionReadOnlyProperty);

    public static void SetExtensionReadOnly(DataGrid obj, bool value)
        => obj.SetValue(ExtensionReadOnlyProperty, value);

    public static readonly DependencyProperty ExtensionReadOnlyProperty =
        DependencyProperty.RegisterAttached("ExtensionReadOnly", typeof(bool), typeof(DataGridExtensionDataColumn), new FrameworkPropertyMetadata(false, (d, e) =>
        {
            if (d is DataGrid dg)
            {
                var readOnly = GetExtensionReadOnly(dg);

                foreach (var c in dg.Columns)
                {
                    if (c is DataGridExtensionDataColumn)
                    {
                        c.IsReadOnly = readOnly;
                    }
                }
            }
        }));

    #endregion ExtensionReadOnly

    private static void GenerateColumns(DataGrid dg)
    {
        var nv = GetExtensionColumns(dg)?.ToList();

        for (var i = dg.Columns.Count - 1; i >= 0; i--)
        {
            if (dg.Columns[i] is DataGridExtensionDataColumn)
            {
                dg.Columns.RemoveAt(i);
            }
        }

        var t = GetExtensionDataType(dg);
        if (nv?.Count > 0 && GetExtensionDataPath(dg) != null && t != null)
        {
            var readOnly = GetExtensionReadOnly(dg);
            var b = new Binding(GetExtensionDataPath(dg));
            foreach (var ec in nv)
            {
                dg.Columns.Add(new DataGridExtensionDataColumn()
                {
                    Header = ec,

                    ExtensionDataType = t,
                    ExtensionData = b,
                    ExtensionDataKey = ec,
                    IsReadOnly = readOnly
                });
            }
        }
    }
}
