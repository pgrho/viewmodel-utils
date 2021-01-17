using System;

namespace Shipwreck.ViewModelUtils
{
    public interface IRequestFocus
    {
        event Action<string> FocusRequested;
    }
}
