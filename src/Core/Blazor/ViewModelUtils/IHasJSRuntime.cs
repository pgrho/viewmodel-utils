using Microsoft.JSInterop;

namespace Shipwreck.ViewModelUtils
{
    public interface IHasJSRuntime
    { 
        IJSRuntime JS { get; }
    }
}
