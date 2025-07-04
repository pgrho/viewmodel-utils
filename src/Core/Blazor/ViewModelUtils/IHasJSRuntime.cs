using Microsoft.AspNetCore.Components;

namespace Shipwreck.ViewModelUtils;

public interface IHasJSRuntime
{
    IJSRuntime JS { get; }
}
