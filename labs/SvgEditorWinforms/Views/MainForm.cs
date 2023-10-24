using Svg;
using System.Diagnostics;
using SvgEditorWinForms;
using Point = System.Drawing.Point;
using SvgEditorWinForms.Models;

namespace SvgDemoWinForms
{
    public partial class MainForm : Form
    {
        public ShapeDrawingController ShapeDrawingController { get; } 

        public JsonViewForm JsonView { get; } 
        public SvgViewForm SvgView { get; } 
        public TreeViewForm TreeView { get; } 
        public LogViewForm LogView { get; } 

        public UndoController<DocumentModel> UndoController { get; }
        public DrawModeController DrawModeController { get; } = new();
        public DataModelController DataModelController { get; } = new();
        public LoggingController LoggingController { get; } = new();
        public FileController FileController { get; }
        public SelectionController SelectionController { get; } = new();
        public ClipboardController ClipboardController { get; } = new();

        public DocumentModel Model
        {
            get => DataModelController.Model;
            set => DataModelController.Model = value;
        }

        public MainForm()
        {
            InitializeComponent();

            FileController = new("json", OnSave, OnOpen);
            FileController.ModifiedChanged += FileController_ModifiedChanged;

            // Clear the text of the tool strip status label
            statusLabel.Text = "";

            // Model Controller initialization
            DataModelController.ModelChanged += DataModelController_ModelChanged;

            // Draw Mode Controller initialization
            DrawModeController.ModeChanged += DrawModeControllerOnModeChanged;
            DrawModeController.Mode = Mode.DrawRect;

            // Undo controller initialization 
            UndoController = new UndoController<DocumentModel>(Model);
            UndoController.UndoStateChanged += UndoControllerOnUndoStateChanged;
            UpdateUndoRedoMenuStates();

            // Initialize the shape drawing controller 
            ShapeDrawingController = new ShapeDrawingController(pictureBox1);

            // TODO: there should be a controller for this, and whether or not they are shown (and where) 
            // should be in the settings. 
            // There should be a settings controller, and a settings model, and a 
            // FormStateController 

            // Create the views 
            JsonView = new JsonViewForm();
            SvgView = new SvgViewForm();
            TreeView = new TreeViewForm(SelectionController, DataModelController);
            LogView = new LogViewForm();

            // When each view is shown/hidden update the check box in the menu
            JsonView.VisibleChanged += (sender, args) => jsonFormToolStripMenuItem.Checked = JsonView.Visible;
            SvgView.VisibleChanged += (sender, args) => svgFormToolStripMenuItem.Checked = SvgView.Visible;
            TreeView.VisibleChanged += (sender, args) => treeFormToolStripMenuItem.Checked = TreeView.Visible;
            LogView.VisibleChanged += (sender, args) => logFormToolStripMenuItem.Checked = LogView.Visible;

            // Prevent forms from being deleted, when they are hidden. 
            JsonView.PreventFormDisposal();
            SvgView.PreventFormDisposal();
            TreeView.PreventFormDisposal();
            LogView.PreventFormDisposal();

            // Show all of the forms initially. 
            SvgView.Show();
            JsonView.Show();
            TreeView.Show();
            LogView.Show();

            // Every time a menu event happens I want to log it
            foreach (var m in AllMenuItems())
            {
                m.Click += MenuItemClicked;
                m.EnabledChanged += MenuItemEnabledChanged;
                m.CheckedChanged += MenuItemCheckedChanged;
            }

            // Initialize the logging controller
            LoggingController.LogUpdated += LoggingServiceOnLogUpdated;
            LoggingController.Log("Starting up");

            // Selection changed handler
            SelectionController.SelectionChanged += SelectionController_SelectionChanged;

            // Clipboard changed
            ClipboardController.ClipboardChanged += ClipboardController_ClipboardChanged;
        }

        private void ClipboardController_ClipboardChanged(object? sender, EventArgs e)
        {
            // ClipboardController
        }

        public bool IsSelected(ElementModel e)
        {
            return SelectionController.SelectedShapes.Contains(e);
        }

        private void SelectionController_SelectionChanged(object? sender, EventArgs e)
        {
            foreach (var model in Model.AllElements().OfType<SimpleShapeModel>())
            {
                model.StrokeColor = IsSelected(model) ? ColorModel.Red : ColorModel.Black;
            }
            DocumentChanged();
        }

        private void FileController_ModifiedChanged(object? sender, EventArgs e)
        {
            modifiedStatusLabel.Text = FileController.Modified ? "Modified" : "Unmodified";
        }

        private void DrawModeControllerOnModeChanged(object? sender, EventArgs e)
        {
            circleToolStripMenuItem.Checked = DrawModeController.Mode == Mode.DrawCircle;
            squareToolStripMenuItem.Checked = DrawModeController.Mode == Mode.DrawSquare;
            ellipseToolStripMenuItem.Checked = DrawModeController.Mode == Mode.DrawEllipse;
            rectangleToolStripMenuItem.Checked = DrawModeController.Mode == Mode.DrawRect;
            selectToolStripMenuItem.Checked = DrawModeController.Mode == Mode.Select;
        }

        public void MenuItemClicked(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                LogInfo($"Menu item {menuItem.Text} clicked ");
            }
        }

        private void MenuItemCheckedChanged(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                LogInfo($"Menu item {menuItem.Text} checked state changed to {menuItem.Checked}");
            }
        }

        private void MenuItemEnabledChanged(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                LogInfo($"Menu item {menuItem.Text} enabled state changed to {menuItem.Enabled}");
            }
        }

        private void LoggingServiceOnLogUpdated(object? sender, EventArgs e)
        {
            LogView.Add(LoggingController.LastMessage);
        }

        public IEnumerable<ToolStripMenuItem> AllMenuItems()
            => menuStrip1.Items.AllMenuItems();
        
        private void DataModelController_ModelChanged(object? sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            UpdateUndoRedoMenuStates();
        }

        private void UndoControllerOnUndoStateChanged(object? sender, EventArgs e)
        {
            LoggingController.Log("Undo controller state updated");
            UpdateUndoRedoMenuStates();
        }

        public void UpdateUndoRedoMenuStates()
        {
            undoToolStripMenuItem.Enabled = UndoController.CanUndo();
            redoToolStripMenuItem.Enabled = UndoController.CanRedo();
        }

        public void LogInfo(string message)
        {
            LoggingController.Log(message);
            statusLabel.Text = message;
        }

        public void OnSave(string fileName)
        {
            var doc = Model.ToJson();
            File.WriteAllText(fileName, doc);
        }

        public void OnOpen(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var doc = DocumentModel.FromJson(text);
            Model = doc;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New();
        }

        public void New()
        {
            if (FileController.NewWithCheck())
                return;

            var model = new DocumentModel();
            UndoController.Reset(model);
            DataModelController.Model = model;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileController.Open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileController.SaveWithCurrentName();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataModelController.Model = UndoController.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataModelController.Model = UndoController.Redo();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var shape in SelectionController.SelectedShapes)
            {
                Model.Remove(shape);
            }
            SelectionController.ClearSelection();
            ModelChanged();            
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectionController.Select(Model.Elements);
        }

        public SvgDocument GetSvgDocument()
        {
            return Model.ToSvg();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (DrawModeController.Mode)
                {
                    case Mode.DrawEllipse:
                        StartDrawingShape(new EllipseModel());
                        break;
                    case Mode.DrawLine:
                        StartDrawingShape(new LineModel());
                        break;
                    case Mode.DrawRect:
                        StartDrawingShape(new RectModel());
                        break;
                    case Mode.DrawCircle:
                        StartDrawingShape(new CircleModel());
                        break;
                    case Mode.DrawSquare:
                        StartDrawingShape(new SquareModel());
                        break;
                    case Mode.Select:
                        SelectWithMouse();
                        break;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                CancelDrawingShape();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateMousePositionStatus();
            UpdateShape();
        }

        public Point MousePositionRelativeToPicture()
        {
            var pt = pictureBox1.PointToScreen(Point.Empty);
            return new Point(MousePosition.X - pt.X, MousePosition.Y - pt.Y);
        }

        public void UpdateMousePositionStatus()
        {
            var pt = MousePositionRelativeToPicture();
            statusLabelMouse.Text = $"X = {pt.X}, Y = {pt.Y}";
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Capture = false;
            CompleteDrawingShape();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var svgDoc = GetSvgDocument();
            svgDoc.Draw(e.Graphics);
        }

        private void svgFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SvgView.Visible = !SvgView.Visible;
        }

        private void jsonFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JsonView.Visible = !JsonView.Visible;
        }

        private void logFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogView.Visible = !LogView.Visible;
        }

        private void treeFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeView.Visible = TreeView.Visible;
        }

        public void DocumentChanged()
        {
            pictureBox1.Invalidate();
            if (JsonView.Visible)
            {
                JsonView.Update(Model);
            }
            if (SvgView.Visible)
            {
                SvgView.Update(Model);
            }
            if (TreeView.Visible)
            {
                TreeView.Update(Model);
            }
        }

        private void ellipseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DrawModeController.Mode = Mode.DrawEllipse;
        }

        private void rectangleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DrawModeController.Mode = Mode.DrawRect;
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawModeController.Mode = Mode.DrawLine;
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawModeController.Mode = Mode.Select;
        }

        public void StartDrawingShape(SimpleShapeModel shape)
        {
            LoggingController.Log($"Starting to draw {shape.Name}");
            Model.Elements.Add(shape);
            ShapeDrawingController.StartDrawing(shape, MousePosition);
            pictureBox1.Capture = true;
            DocumentChanged();
        }

        public void UpdateUndoController()
        {
            FileController.Modified = true;
            UndoController.NewItem(Model.Clone());
        }

        public void CompleteDrawingShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            ShapeDrawingController.StopDrawing();
            UpdateUndoController();
            DocumentChanged();
        }

        public void CancelDrawingShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            Model.Elements.Remove(ShapeDrawingController.Shape);
            ShapeDrawingController.StopDrawing();
            DocumentChanged();
        }

        public void UpdateShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            ShapeDrawingController.Update(MousePosition);
            DocumentChanged();
        }

        public string GetXml()
        {
            return GetSvgDocument().GetXML();
        }

        private void showInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = Path.ChangeExtension(Path.GetTempFileName(), "svg");
            File.WriteAllText(filePath, GetXml());
            var  psi = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void exportSVGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            File.WriteAllText(saveFileDialog1.FileName, GetXml());
        }

        public Bitmap RenderToBitmap(int width, int height)
        {
            var bmp = new Bitmap(width, height);
            GetSvgDocument().Draw(bmp);
            return bmp;
        }

        private void exportPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            var bmp = RenderToBitmap(pictureBox1.Width, pictureBox1.Height);
            bmp.Save(saveFileDialog1.FileName);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Model.Elements.Clear();
            ModelChanged();
        }

        public void ModelChanged()
        {
            DocumentChanged();
            UpdateUndoController();
        }

        public bool HitTest(ElementModel e, Point pt)
        {
            if (e is SimpleShapeModel ssm)
            {
                return (pt.X >= ssm.Left && pt.X <= ssm.Right)
                       && (pt.Y >= ssm.Top && pt.Y <= ssm.Bottom);
            }

            return false;
        }

        public ElementModel? HitTest(Point pt)
        {
            foreach (var x in Model.AllElements().Reverse())
            {
                if (HitTest(x, pt))
                    return x;
            }

            return null;
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawModeController.Mode = Mode.DrawSquare;
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawModeController.Mode = Mode.DrawCircle;
        }

        public void SelectWithMouse()
        {
            var e = HitTest(MousePositionRelativeToPicture());
            if (e != null)
            {
                SelectionController.Select(e);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileController.SaveWithDialog();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CancelDrawingShape();
            }
        }
    }
}