using System.Windows.Documents;

namespace Shipwreck.ViewModelUtils.Controls;

public static class TextElementBehavior
{
    public static TextDecorationCollection GetTextDecorations(DependencyObject obj) => (TextDecorationCollection)obj.GetValue(TextDecorationsProperty);

    public static void SetTextDecorations(DependencyObject obj, TextDecorationCollection value) => obj.SetValue(TextDecorationsProperty, value);

    public static readonly DependencyProperty TextDecorationsProperty
        = DependencyProperty.RegisterAttached("TextDecorations", typeof(TextDecorationCollection), typeof(TextElementBehavior), new PropertyMetadata(null, (d, e) =>
        {
            if (d is TextBlock textBlock)
            {
                textBlock.TextDecorations = e.NewValue as TextDecorationCollection;
            }
            else if (d is TextBox textBox)
            {
                textBox.TextDecorations = e.NewValue as TextDecorationCollection;
            }
            else if (d is Paragraph p)
            {
                p.TextDecorations = e.NewValue as TextDecorationCollection;
            }
            else if (d is Inline i)
            {
                i.TextDecorations = e.NewValue as TextDecorationCollection;
            }
            else if (d is AccessText a)
            {
                a.TextDecorations = e.NewValue as TextDecorationCollection;
            }
        }));
}
