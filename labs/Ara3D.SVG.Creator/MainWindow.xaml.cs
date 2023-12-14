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

namespace Ara3D.SVG.Creator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Vector2 V1 = Vector2.Zero;
        public Vector2 V2 = Vector2.Zero;

        public SvgPath Path; 

        public MainWindow()
        {
            InitializeComponent();
            PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;
            Browser.CoreWebView2InitializationCompleted += BrowserOnCoreWebView2InitializationCompleted;
            Browser.EnsureCoreWebView2Async();
            Browser.WebMessageReceived += BrowserOnWebMessageReceived;
            Browser.PreviewMouseDown += Browser_PreviewMouseDown;
            MouseDown += OnMouseDown;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Browser_PreviewMouseDown(sender, e);
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

            var bldr = new PathBuilder();
            bldr.MoveAbs(V1);
            bldr.LineAbs(V2);
            Path = bldr.Path;

            var xml = bldr.ToXml();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var text = xmlDoc.DocumentElement.OuterXml.Replace("\"", "\\\"");
            Debug.WriteLine(text);

            var script = $"setSvgText(\"{text}\")";
            this.Browser.CoreWebView2.ExecuteScriptAsync(script);
            //Debug.WriteLine($"x = {}");
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
    elem.appendChild(xmlDoc.documentElement);    
    //elem.textContent = text;
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
        
        public static string GetMouseScript = @"'Cursor at: '+cursor_x+', '+cursor_y";

        private async void Browser_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var result = await Browser.CoreWebView2.ExecuteScriptAsync(GetMouseScript);
            Debug.WriteLine(result);
        }

        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Double clicked");
            LoadSvg();
        }

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