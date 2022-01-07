namespace Shipwreck.ViewModelUtils;

internal sealed class GenericMetroDialog : BaseMetroDialog
{
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (!e.Handled)
        {
            if (e.Key == Key.Escape || (e.Key == Key.System && e.SystemKey == Key.F4))
            {
                (DataContext as WindowViewModel)?.CloseCommand.Execute();
            }
        }
    }
}
