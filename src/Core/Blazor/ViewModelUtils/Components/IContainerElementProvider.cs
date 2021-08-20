using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils.Components
{
    public interface IContainerElementProvider
    {
        ElementReference Container { get; }
    }
}
