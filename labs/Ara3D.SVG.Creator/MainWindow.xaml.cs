using System.Diagnostics; 
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Ara3D.Collections;
using Ara3D.Math;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Svg;
using Svg.Pathing;

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

        public Stack CurrentStack { get; set; } = new Stack();

        public MainWindow()
        {
            InitializeComponent();
            Browser.CoreWebView2InitializationCompleted += BrowserOnCoreWebView2InitializationCompleted;
            Browser.EnsureCoreWebView2Async();
            Browser.WebMessageReceived += BrowserOnWebMessageReceived;
            
            // <local:PropertiesPanel x:Name="Props1"></local:PropertiesPanel>
            StackPanel.Children.Add(PropertiesPanel = new PropertiesPanel());
            PropertiesPanel.DataObject = CurrentStack;
            PropertiesPanel.PropertyChanged += PropertiesPanel_PropertyChanged;
        }

        private void PropertiesPanel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RedrawSvg();
        }

        public void RedrawSvg()
        {
            var group = new SvgGroup();
            if (CurrentStack.Cloner != null)
            {
                var stacks = CurrentStack.Cloner.Clone(CurrentStack);
                foreach (var stack in stacks.ToEnumerable())
                {
                    var subGroup = RebuildPath(stack);
                    group.Children.Add(subGroup);
                }
            }
            else
            {
                group = RebuildPath(CurrentStack);
            }
            SetXml(group.GetXML());
        }
        
        public SvgGroup RebuildPath(Stack stack)
        {
            var group = new SvgGroup();
            var n = stack.RendererParameters.NumSamples;

            if (stack.RendererParameters.AsPointsOrLines)
            {
                group.Children.Clear();

                for (var i = 0f; i <= n; ++i)
                {
                    var v = stack.GetPoint(i / n);

                    var circle = new SvgCircle();
                    circle.CenterX = v.X;
                    circle.CenterY = v.Y;
                    circle.Radius = (float)stack.RendererParameters.OuterThickness;
                    circle.Fill = new SvgColourServer(stack.RendererParameters.StrokeColor);
                    group.Children.Add(circle);
                }

                for (var i = 0f; i <= n; ++i)
                {
                    var v = stack.GetPoint(i / n);

                    var circle = new SvgCircle();
                    circle.CenterX = v.X;
                    circle.CenterY = v.Y;
                    circle.Radius = (float)stack.RendererParameters.InnerThickness;
                    circle.Fill = new SvgColourServer(stack.RendererParameters.FillColor);
                    group.Children.Add(circle);
                }
            }
            else
            {
                var path1 = new SvgPath();
                var path2 = new SvgPath();

                path1.PathData = new SvgPathSegmentList();

                var v = stack.GetPoint(0);
                path1.PathData.Add(new SvgMoveToSegment(false, v.ToSvg()));
                for (var i = 1f; i <= n; i += 1)
                {
                    v = stack.GetPoint(i / n);
                    path1.PathData.Add(new SvgLineSegment(false, v.ToSvg()));
                }

                path1.StrokeWidth = (float)stack.RendererParameters.OuterThickness;
                path1.Fill = SvgPaintServer.None;
                path1.Stroke = new SvgColourServer(stack.RendererParameters.FillColor);

                path2.PathData = path1.PathData;
                path2.StrokeWidth = (float)stack.RendererParameters.InnerThickness;
                path2.Fill = SvgPaintServer.None;
                path2.Stroke = new SvgColourServer(stack.RendererParameters.StrokeColor);

                group.Children.Clear();
                group.Children.Add(path1);
                group.Children.Add(path2);
            }

            return group;
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

            CurrentStack.A = CurrentStack.B;
            float x = json.Pos.x;
            float y = json.Pos.y;
            CurrentStack.B = new Vector2(x, y);

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
            
        public static string TrackMouseScript = @"var cursor_x = -1;
var cursor_y = -1;
document.onmousemove = function(event)
{
 cursor_x = event.pageX;
 cursor_y = event.pageY;
}";
        
        private async void BrowserOnCoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            Debug.WriteLine("Web-browser Initialized!");
            Browser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(OnDocCreatedScript);
            LoadSvg();
            await Browser.CoreWebView2.ExecuteScriptAsync(TrackMouseScript);
        }

        public void LoadSvg()
        {
            //var text = File.ReadAllText(@"C:\Users\cdigg\git\ara3d-dev\ara3d.github.io\ara3d.svg");
            //var text2 = File.ReadAllText(@"C:\Users\cdigg\Downloads\SVG spilled Water Effect.html");
            Browser.NavigateToString(Html);

            // TODO: this will be fun.
            // this.Browser.CoreWebView2.ExecuteScriptAsync("");

            // TODO: so will this
            //this.Browser.CoreWebView2.PrintToPdfAsync()

            // TODO: this ight be useful
            //this.Browser.CoreWebView2.AddHostObjectToScript("name", null);
        }

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
       }
    }
}