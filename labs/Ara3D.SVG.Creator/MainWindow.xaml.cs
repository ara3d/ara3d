using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Ara3D.Math;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public Vector2 V1 = Vector2.Zero;
        public Vector2 V2 = Vector2.Zero;

        public SvgPath Path1 = new SvgPath();
        public SvgPath Path2 = new SvgPath();
        public SvgGroup Group = new SvgGroup();

        public readonly FunctionRendererParameters RendererParameters = new FunctionRendererParameters();

        public readonly PropertiesPanel PropertiesPanel;

        public MainWindow()
        {
            InitializeComponent();
            Browser.CoreWebView2InitializationCompleted += BrowserOnCoreWebView2InitializationCompleted;
            Browser.EnsureCoreWebView2Async();
            Browser.WebMessageReceived += BrowserOnWebMessageReceived;
            
            // <local:PropertiesPanel x:Name="Props1"></local:PropertiesPanel>
            ListBox.Items.Add(PropertiesPanel = new PropertiesPanel());
            PropertiesPanel.DataObject = RendererParameters;
            PropertiesPanel.PropertyChanged += PropertiesPanel_PropertyChanged;
        }

        private void PropertiesPanel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RedrawSvg();
        }

        public void RedrawSvg()
        {
            RebuildPath();
            SetXml(Group.GetXML());
        }

        public void RebuildPath()
        {
            Path1.PathData = new SvgPathSegmentList();
            Path1.PathData.Add(new SvgMoveToSegment(false, V1.ToSvg()));
            var d = (V2 - V1).Length();

            var n = RendererParameters.NumSamples;
            var delta = (V2 - V1) / n;
            var amp = d / 2;

            for (var i = 0; i < n; ++i)
            {
                var theta = i * 2 * System.Math.PI / n;
                var y = (float)System.Math.Sin(theta) * amp;
                var baseLine = V1 + delta * i;
                var pos = baseLine + (0,  y);
                Path1.PathData.Add(new SvgLineSegment(false, pos.ToSvg()));
            }

            Path1.StrokeWidth = (float)RendererParameters.OuterThickness;
            Path1.Fill = SvgPaintServer.None;
            Path1.Stroke = new SvgColourServer(RendererParameters.FillColor);

            Path2.PathData = Path1.PathData;
            Path2.StrokeWidth = (float)RendererParameters.InnerThickness;
            Path2.Fill = SvgPaintServer.None;
            Path2.Stroke = new SvgColourServer(RendererParameters.StrokeColor);

            Group.Children.Clear();
            Group.Children.Add(Path1);
            Group.Children.Add(Path2);
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

            V1 = V2;
            float x = json.Pos.x;
            float y = json.Pos.y;
            V2 = new Vector2(x, y);

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
    }
}