using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using WinRT.Interop;
#endif

namespace Calculator2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            const int fixedWidth = 450;
            const int fixedHeight = 600;

            window.Width = fixedWidth;
            window.Height = fixedHeight;
            window.MinimumWidth = fixedWidth;
            window.MaximumWidth = fixedWidth;
            window.MinimumHeight = fixedHeight;
            window.MaximumHeight = fixedHeight;


#if WINDOWS
            window.HandlerChanged += (s, e) =>
            {
                var nativeWindow = window.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
                if (nativeWindow is not null)
                {
                    var hwnd = WindowNative.GetWindowHandle(nativeWindow);
                    DisableMaximizeButton(hwnd);
                }
            };
#endif

            return window;
        }




#if WINDOWS
        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x00010000;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private static void DisableMaximizeButton(IntPtr hwnd)
        {
            var style = GetWindowLong(hwnd, GWL_STYLE);
            style &= ~WS_MAXIMIZEBOX;  // убираем флаг maximize
            SetWindowLong(hwnd, GWL_STYLE, style);
        }
#endif
    }
}