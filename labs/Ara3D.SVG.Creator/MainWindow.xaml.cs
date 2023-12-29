using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Ara3D.Math;
using Ara3D.Utils.Wpf;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Svg;

//== 
// https://stackoverflow.com/questions/20810578/setting-attribute-of-svg-element-with-js-not-working?rq=3


namespace Ara3D.SVG.Creator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly PropertiesPanel PropertiesPanel;

        public List<OperatorStack> Stacks { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            Browser.CoreWebView2InitializationCompleted += BrowserOnCoreWebView2InitializationCompleted;
            Browser.EnsureCoreWebView2Async();
            Browser.WebMessageReceived += BrowserOnWebMessageReceived;
            StackPanel.Children.Add(PropertiesPanel = new PropertiesPanel());
            PropertiesPanel.PropertyChanged += PropertiesPanel_PropertyChanged;
            CreateMenu();
        }

        public void SetCurrentEntity(OperatorStack operatorStack)
        {
            PropertiesPanel.DataObject = operatorStack;
            PropertiesPanel.RecomputeLayout();
        }

        public void CreateObject(Generator gen)
        {
            var entity = new OperatorStack();
            Stacks.Add(entity);
            entity.Generator = gen;
            /*
            var setStroke = new SetStrokeColor() { Color = Color.Black };
            entity.Operators.Add(setStroke);

            var setStrokeWidth = new SetStrokeWidth() { Width = 3 };
            entity.Operators.Add(setStrokeWidth);

            var setFillColor = new SetFillColor() { Color = Color.AntiqueWhite };
            entity.Operators.Add(setFillColor);
            */
            SetCurrentEntity(entity);
            RedrawSvg();
        }

        public void AddModifier(Operator mod)
        {
            var entity = Stacks.LastOrDefault();
            if (entity == null) return;
            entity.Operators.Add(mod);
            SetCurrentEntity(entity);
        }

        public void AddModifierMenuItem<T>(MenuItem parent) where T: Operator, new()
        {
            var name = typeof(T).Name;
            parent.AddMenuItem(name, () => AddModifier(new T()));
        }

        public void AddCreateMenuItem<T>(MenuItem parent) where T : Generator, new()
        {
            var name = typeof(T).Name;
            parent.AddMenuItem(name, () => CreateObject(new T()));
        }

        public void CreateMenu()
        {
            this.Menu.Items.Clear();
            var create = this.Menu.AddMenuItem("Create");
            AddCreateMenuItem<RectGenerator>(create);
            AddCreateMenuItem<EllipseGenerator>(create);
            AddCreateMenuItem<SquareGenerator>(create);
            AddCreateMenuItem<CircleGenerator>(create);
            AddCreateMenuItem<RawSvg>(create);
            AddCreateMenuItem<StarShape>(create);
            var mods = this.Menu.AddMenuItem("Modify");
            AddModifierMenuItem<SetStroke>(mods);
            AddModifierMenuItem<SetFillColor>(mods);
            AddModifierMenuItem<TransformOperator>(mods);
            var clones = this.Menu.AddMenuItem("Cloners");
            AddModifierMenuItem<Cloner>(clones);
        }

        private void PropertiesPanel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RedrawSvg();
        }

        public void RedrawSvg()
        {
            var doc = new SvgDocument();
            foreach (var stk in Stacks)
                doc.Children.Add(stk.Evaluate().Svg);
            SetXml(doc.GetXML());
        }

        public void SetSvg(string svg)
        {
            var text = svg.Replace("\"", "\\\"");
            var script = $"setSvgText(\"{text}\")";
            Browser.CoreWebView2.ExecuteScriptAsync(script);
        }

        public void SetXml(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            SetSvg(xmlDoc.DocumentElement.OuterXml);
        }

        private void BrowserOnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var txt = e.WebMessageAsJson;
            Debug.WriteLine(e.WebMessageAsJson);
            //var jo = JObject.Parse(txt);

            dynamic json = JsonConvert.DeserializeObject(txt);

            float x = json.Pos.x;
            float y = json.Pos.y;

            var stack = Stacks.LastOrDefault();
            if (stack == null) return;

            stack.Generator.A = stack.Generator.B;
            stack.Generator.B = new DVector2(x, y);

            var (ax, ay) = stack.Generator.A.ToVector();
            var (bx, by) = stack.Generator.B.ToVector();
            var minX = System.Math.Min(ax, bx);
            var minY = System.Math.Min(ay, by);
            var maxX = System.Math.Max(ax, bx);
            var maxY = System.Math.Max(ay, by);

            stack.Generator.A = new DVector2(minX, minY);
            stack.Generator.B = new DVector2(maxX, maxY);

            RedrawSvg();
            /*
            var bldr = new PathBuilder();
            bldr.MoveAbs(V1);
            bldr.LineAbs(V2);
            Path = bldr.Path;

            SetXml(bldr.ToXml());
            */
        }

        public static string Html = 
@"<html style='width:100%;height:100%;'>
<body style='width:100%;height:100%;margin:0;'>
  <svg height='100%' width='100%' id='svgMainId'>
  </svg>
</body>
</html>";

        public static string OnDocCreatedScript = @"
// https://stackoverflow.com/a/74087257/184528
// https://stackoverflow.com/questions/29261304/how-to-get-the-click-coordinates-relative-to-svg-element-holding-the-onclick-lis
// https://stackoverflow.com/a/42711775/184528
function eventToSvgCoords(evt, svgId) {
    let elem = document.getElementById(svgId);
    let pt = DOMPoint.fromPoint(elem); 
    pt.x = evt.clientX;
    pt.y = evt.clientY;
    return pt.matrixTransform(elem.getScreenCTM().inverse());
}

function setSvgText(text)
{
    console.log(""setting SVG text"");
    console.log(text);
    let elem = document.getElementById('svgMainId');
    console.log(elem);
    // https://stackoverflow.com/questions/24079659/loading-svg-using-innerhtml
    var xmlDoc = new DOMParser().parseFromString(text, ""text/xml"");
    //elem.     
    elem.textContent = '';
    elem.appendChild(xmlDoc.documentElement);    
    //elem.remove(elem.firstChild);
    //elem.textContent = text;
    //elem.textContent = xmlDoc.documentElement;
}

function onEvent(event)
{
    let elem = event.target;
    let pos = eventToSvgCoords(event, 'svgMainId');
    let jsonObject =
    {
        Key: event.type,
        Pos: pos,
        Value: elem.name || elem.id || elem.tagName || 'Unknown'
    };
    window.chrome.webview.postMessage(jsonObject);
}

// List of events is here: https://www.w3schools.com/jsref/dom_obj_event.asp
document.addEventListener('click', onEvent);
document.addEventListener('dblclick', onEvent);
";
        
        private async void BrowserOnCoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            Debug.WriteLine("Web-browser Initialized!");
            Browser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(OnDocCreatedScript);
            LoadSvg();

            var rg = new RectGenerator();
            rg.A = new DVector2(50, 50);
            rg.B = new DVector2(250, 250);
            CreateObject(rg);
            AddModifier(new TransformOperator());   
            AddModifier(new SetStroke());
            AddModifier(new SetFillColor());
            RedrawSvg();
        }

        public void LoadSvg()
        {
            //var text = File.ReadAllText(@"C:\Users\cdigg\git\ara3d-dev\ara3d.github.io\ara3d.svg");
            //var text2 = File.ReadAllText(@"C:\Users\cdigg\Downloads\SVG spilled Water Effect.html");
            Browser.NavigateToString(Html);

            // TODO: this will be fun.
            //this.Browser.CoreWebView2.PrintToPdfAsync()

            // TODO: this might be useful
            //this.Browser.CoreWebView2.AddHostObjectToScript("name", null);
        }
        /*
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
           if (sender is MenuItem mi)
           {
               var s = mi.Header as string;
               switch (s?.ToLowerInvariant())
               {
                    case "sine wave":
                        CurrentStack.Function = new SineWave();
                        break;
                    case "circle":
                        CurrentStack.Function = new Circle();
                        break;
                    case "rose":
                        CurrentStack.Function = new Rose();
                        break;
                    case "spiral":
                        CurrentStack.Function = new Spiral();
                        break;
                    case "gaussian":
                        CurrentStack.Function = new Gaussian();
                        break;
               }
               PropertiesPanel.RecomputeLayout();
               RedrawSvg();
            }
           //throw new NotImplementedException();
       }*/
    }
}