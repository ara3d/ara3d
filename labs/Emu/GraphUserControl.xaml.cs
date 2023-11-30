using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Peacock;
using Peacock.Wpf;

namespace Emu;

/// <summary>
/// Interaction logic for GraphUserControl.xaml
/// TODO: a lot of this could go into the Peacock.Wpf layer.
/// </summary>
public partial class GraphUserControl : UserControl
{
    public ControlManager Manager;
    public IControlFactory Factory;
    public Graph Graph;

    public WpfCanvas Canvas = new();
    public int WheelZoom;
    public double ZoomFactor => Math.Pow(1.15, WheelZoom / 120.0);

    public DispatcherTimer Timer = new()
    {
        Interval = TimeSpan.FromMilliseconds(20)
    };

    public DateTimeOffset Started;

    public class MouseStatus : IMouseStatus
    {
        public MouseStatus(GraphUserControl control) => Control = control;
        public GraphUserControl Control { get; }
        public Point Location => Mouse.GetPosition(Control).Multiply(1.0 / Control.ZoomFactor);
        public bool LButtonDown => Mouse.LeftButton == MouseButtonState.Pressed;
        public bool RButtonDown => Mouse.RightButton == MouseButtonState.Pressed;
        public bool MButtonDown => Mouse.MiddleButton == MouseButtonState.Pressed;
    }

    public GraphUserControl()
    {
        InitializeComponent();
        Focusable = true;
        Focus();

        Graph = TestData.CreateGraph();
        Factory = new ControlFactory();
        Manager = new ControlManager(Factory);
        Manager.UpdateControlTree(Graph, new Rect(RenderSize));

        //(this.Parent as Window).PreviewKeyDown += (sender, args) => Console.WriteLine("Parent key press");
        PreviewKeyDown += (sender, args) => ProcessInput(new KeyDownEvent(args));
        PreviewKeyUp += (sender, args) => ProcessInput(new KeyUpEvent(args));
        PreviewMouseDoubleClick += (sender, args) => ProcessInput(new MouseDoubleClickEvent(args));
        PreviewMouseDown += (sender, args) => ProcessInput(new MouseDownEvent(args));
        PreviewMouseUp += (sender, args) => ProcessInput(new MouseUpEvent(args));
        PreviewMouseMove += (sender, args) => ProcessInput(new MouseMoveEvent(args));
        PreviewMouseWheel += (sender, args) => ProcessInput(new MouseWheelEvent(args));
        SizeChanged += (sender, args) => ProcessInput(new ResizeEvent(args));
            
        // Animation timer
        Timer.Tick += (sender, args) => ProcessInput(new ClockEvent((DateTimeOffset.Now - Started).TotalSeconds));
        Timer.Start();
        Started = DateTimeOffset.Now;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        Render(drawingContext);
        base.OnRender(drawingContext);
    }

    public void Render(DrawingContext drawingContext)
    {
        var rect = new Rect(new(), RenderSize);
        drawingContext.PushClip(new RectangleGeometry(rect));
        drawingContext.DrawRectangle(new SolidColorBrush(Colors.SlateGray), new Pen(), rect);
        var scaleTransform = new ScaleTransform(ZoomFactor, ZoomFactor);
        drawingContext.PushTransform(scaleTransform);
        Canvas.Context = drawingContext;
        Render(Canvas);
        drawingContext.Pop();
        drawingContext.Pop();
    }

    public void Render(ICanvas canvas)
    {
        Manager.Draw(canvas);
    }

    public void ProcessInput<T>(T inputEvent)
        where T : InputEvent
    {
        if (inputEvent is MouseWheelEvent mwe)
        {
            WheelZoom += mwe.Args.Delta;
        }

        inputEvent.MouseStatus = new MouseStatus(this);

        var updates = Manager.ProcessInput(inputEvent);
        Manager.ApplyChanges(updates);
        Graph = updates.UpdateModel(Graph);
        Manager.UpdateControlTree(Graph, new Rect(new(), new Size(10000, 10000)));
        InvalidateVisual();
    }
}