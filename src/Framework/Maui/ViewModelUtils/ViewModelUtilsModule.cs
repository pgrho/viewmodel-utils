using Microsoft.Maui.Hosting;

namespace Shipwreck.ViewModelUtils;

public static class ViewModelUtilsModule
{
    public static void Init()
    {
    }

    public static IMauiHandlersCollection AddViewModelUtilsHandlers(this IMauiHandlersCollection handlers)
    {
#if ANDROID
        handlers.AddHandler(typeof(SelectableEntry), typeof(SelectableEntryRenderer));
#endif
        return handlers;
    }
}
