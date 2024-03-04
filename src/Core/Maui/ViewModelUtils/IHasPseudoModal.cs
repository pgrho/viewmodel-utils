namespace Shipwreck.ViewModelUtils;

public interface IHasPseudoModal
{
    bool TryOpen(object viewModel);

    bool TryClose(object viewModel);
}
