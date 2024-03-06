using Microsoft.Extensions.Logging;
using Shipwreck.BootstrapControls;
using Shipwreck.FontAwesomeControls;

namespace Shipwreck.ViewModelUtils.Demo.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFontAwesome())
            .ConfigureMauiHandlers(h =>
            {
                h.AddBootstrapHandlers();
                h.AddViewModelUtilsHandlers();
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
