using System.Windows;

namespace Ara3D.FloorPlanner
{
    public class MouseButtonState
    {
        public bool Down;
        public bool Clicked;
        public bool DoubleClicked;
    }

    public class WindowState
    {
        public Rect Rect;
        public bool Minimized;
    }

    /// <summary>
    /// The input that comes into a control.
    /// It can be pre-processed by behaviors 
    /// </summary>
    public class UserInput
    {
        public double CurrentTime;
        public double DeltaTime;
        public List<int> KeysPressed;
        public Point Mouse;
        public MouseButtonState Left = new();
        public MouseButtonState Middle = new();
        public MouseButtonState Right = new();
        public double DeltaMouseWheel; 
        public WindowState Window = new();
    }   

}