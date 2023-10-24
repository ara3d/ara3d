namespace SvgDemoWinForms
{
    public enum Mode
    {
        Initial,
        Select,
        DrawCircle,
        DrawRect,
        DrawEllipse,
        DrawSquare, 
        DrawLine,
    }

    public class DrawModeController
    {
        private Mode _mode = Mode.Initial;

        public Mode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                ModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? ModeChanged;
    }
}