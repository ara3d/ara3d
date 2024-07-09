using System.Windows;
using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.NodeEditor
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
    /// An abstract representations of input that comes into a control.
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
        public WindowState Window = new();
    }   

}