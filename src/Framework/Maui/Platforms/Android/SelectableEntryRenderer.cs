using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace Shipwreck.ViewModelUtils;

public sealed class SelectableEntryRenderer : EntryRenderer
{
    public SelectableEntryRenderer(Context context) 
        : base(context)
    {
    }
}
