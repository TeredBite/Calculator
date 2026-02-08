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
            return window;
        }
    }
}