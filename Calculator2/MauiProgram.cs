using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
#endif

namespace Calculator2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(w =>
                    {
                        w.OnWindowCreated(window =>
                        {
                            var hwnd = WindowNative.GetWindowHandle(window);
                            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
                            var appWindow = AppWindow.GetFromWindowId(windowId);

                            
                            if (appWindow.Presenter is OverlappedPresenter presenter)
                            {
                                presenter.IsMaximizable = false;
                                presenter.IsResizable = false;
                            }

                            
                            var display = DisplayArea.Primary;
                            int width = 450;
                            int height = 600;
                            int x = (display.WorkArea.Width - width) / 2;
                            int y = (display.WorkArea.Height - height) / 2;

                            appWindow.MoveAndResize(new Windows.Graphics.RectInt32(x, y, width, height));
                        });
                    });
#endif
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}