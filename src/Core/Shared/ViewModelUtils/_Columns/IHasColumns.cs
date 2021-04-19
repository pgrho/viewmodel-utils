using System;
using System.Collections.Generic;

namespace Shipwreck.ViewModelUtils
{
    public interface IHasColumns : IFrameworkPageViewModel
    {
        long DefaultColumns { get; }
        long Columns { get; set; }

        IEnumerable<KeyValuePair<Enum, string>> GetFlags();
    }
}
