﻿using System.Collections.Generic;

namespace Shipwreck.ViewModelUtils
{
    public interface IHasExtensionColumns : IHasColumns
    {
        BulkUpdateableCollection<string> ExtensionColumns { get; }
        IReadOnlyList<string> SelectedExtensionColumns { get; set; }

        IEnumerable<string> GetDefaultExtensionColumns();
    }
}
