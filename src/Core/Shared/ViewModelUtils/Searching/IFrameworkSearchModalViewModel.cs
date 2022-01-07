namespace Shipwreck.ViewModelUtils.Searching;

public interface IFrameworkSearchModalViewModel : IFrameworkModalViewModel
{
    IFrameworkSearchPageViewModel SearchPage { get; }

    void Select(object item);
}
