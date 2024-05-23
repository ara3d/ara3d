using Ara3D.Collections;
using Ara3D.Mathematics;

namespace Ara3D.NodeEditor
{
    public class MouseButtonState
    {
        public readonly bool Down;
        public readonly bool Clicked;
        public readonly bool DoubleClicked;

        public MouseButtonState(bool down, bool clicked, bool doubleClicked)
        {
            Down = down;
            Clicked = clicked;
            DoubleClicked = doubleClicked;
        }
    }

    /// <summary>
    /// An abstract representations of input that comes into a control.
    /// It can be pre-processed by behaviors 
    /// </summary>
    public class UserInput
    {
        public readonly float Time;
        public readonly IArray<int> KeysPressed;
        public readonly Vector2 Mouse;
        public readonly MouseButtonState Left;
        public readonly MouseButtonState Middle;
        public readonly MouseButtonState Right;

        public UserInput(float time, IArray<int> keysPressed, Vector2 mouse, MouseButtonState left, MouseButtonState middle, MouseButtonState right)
        {
            Time = time;
            KeysPressed = keysPressed;
            Mouse = mouse;
            Left = left;
            Middle = middle;
            Right = right;
        }
    }
}