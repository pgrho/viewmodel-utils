using Shipwreck.ViewModelUtils.Demo.Blazor.Pages;
using Shipwreck.ViewModelUtils.Demo.Blazor.ViewModels;

namespace Shipwreck.ViewModelUtils.Demo.Blazor;

public sealed class DemoInteractionService : InteractionService
{
    public DemoInteractionService()
    {
        RegisterModal(typeof(TestModalViewModel), typeof(TestModal));
    }
}
