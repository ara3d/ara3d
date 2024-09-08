using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Ara3D.NodeEditor
{

    /// <summary>
    /// Interaction logic for NodeEditorControl.xaml
    /// </summary>
    public partial class NodeEditorControl : UserControl
    {
        public NodeEditorController Controller { get; }
        
        public Gui Gui { get; }
        public WpfCanvas Canvas = new();
        public UserInput UserInput = new();        
        public int WheelZoom;
        public double ZoomFactor => Math.Pow(1.15, WheelZoom / 120.0);

        public DispatcherTimer Timer = new()
        {
            Interval = TimeSpan.FromMilliseconds(20)
        };

        public DateTimeOffset Started;

        public event EventHandler FrameUpdatedEventHandler;       
        

        public NodeEditorControl()
        {
            InitializeComponent();
            
            // TODO: this should be passed to the NodeEditorControl ...
            Controller = new NodeEditorController();
            Gui = new Gui(Controller);
            var width = this.Width;
            var height = this.Height;
            var rect = new Rect(new Size(width, height));
            Gui.SetNewModel(new SampleModel().Graph(rect));
            
            Focusable = true;
            Focus();

            //(this.Parent as Window).PreviewKeyDown += (sender, args) => Console.WriteLine("Parent key press");
            PreviewKeyDown += (sender, args) => UserEvent();
            PreviewKeyUp += (sender, args) => UserEvent();
            PreviewMouseDoubleClick += (sender, args) => DoubleClickEvent(args);
            PreviewMouseDown += (sender, args) => UserEvent();
            PreviewMouseMove += (sender, args) => UserEvent();
            PreviewMouseWheel += (sender, args) => UserEvent();
            SizeChanged += (sender, args) => UserEvent();

            Started = DateTimeOffset.Now;

            // Animation timer
            Timer.Tick += (sender, args) => UserEvent();
            Timer.Start();
            Started = DateTimeOffset.Now;
        }
               
        protected override void OnRender(DrawingContext drawingContext)
        {
            Render(drawingContext); 
                       
            FrameUpdatedEventHandler?.Invoke(this, EventArgs.Empty);
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
            var newTime = (DateTimeOffset.Now - Started).TotalSeconds;
            UserInput.DeltaTime = newTime - UserInput.CurrentTime;
            UserInput.CurrentTime = newTime;

            var mousePoint = Mouse.GetPosition(this);
            UserInput.Mouse = new Point(mousePoint.X, mousePoint.Y);
            UserInput.Left.Down = Mouse.LeftButton == System.Windows.Input.MouseButtonState.Pressed;
            UserInput.Right.Down = Mouse.RightButton == System.Windows.Input.MouseButtonState.Pressed;
            UserInput.Middle.Down = Mouse.MiddleButton == System.Windows.Input.MouseButtonState.Pressed;

            Gui.OnFrameUpdate(canvas, UserInput);

            // Reset the user user input
            UserInput = new UserInput();
            UserInput.CurrentTime = newTime;
        }

        public void UserEvent()
        {
            InvalidateVisual();
        }

        public void DoubleClickEvent(MouseEventArgs args)
        {
            InvalidateVisual();
        }
    }
}
