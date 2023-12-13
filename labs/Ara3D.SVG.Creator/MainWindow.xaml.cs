using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;

namespace Ara3D.SVG.Creator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            Debug.WriteLine(e.WebMessageAsJson);
        }

        // List of events is here: https://www.w3schools.com/jsref/dom_obj_event.asp
        // 
        public static string OnDocCreatedScript = @"
function onEvent(event)
{
    let elem = event.target;
    let jsonObject =
    {
        Key: event.type,
        Value: elem.name || elem.id || elem.tagName || 'Unknown'
    };
    window.chrome.webview.postMessage(jsonObject);
}

document.addEventListener('click', onEvent);
document.addEventListener('dblclick', onEvent);
";

        // https://stackoverflow.com/questions/29261304/how-to-get-the-click-coordinates-relative-to-svg-element-holding-the-onclick-lis
        // https://stackoverflow.com/a/42711775/184528
        public static string FromSvgPoint = @"var pt = svg.createSVGPoint();  // Created once for document

function alert_coords(evt) {
    pt.x = evt.clientX;
    pt.y = evt.clientY;

    // The cursor point, translated into svg coordinates
    var cursorpt =  pt.matrixTransform(svg.getScreenCTM().inverse());
    console.log(""("" + cursorpt.x + "", "" + cursorpt.y + "")"");
}";

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

        public static string Svg =
            @"<svg version=""1.1"" id=""Ebene_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"">

    <defs>
      <filter id=""filter"" width=""150%"" height=""160%"" x=""-25%"" y=""-25%"">
      <!-- COLORS -->
        <feFlood flood-color=""#16B5FF"" result=""COLOR-blue""></feFlood>‚
        <feFlood flood-color=""#9800FF"" result=""COLOR-violet""></feFlood>
        <feFlood flood-color=""#A64DFF"" result=""COLOR-violet-light""></feFlood>
      <!-- COLORS END -->

      <!-- BOTTOM SPLASH -->
        <feTurbulence baseFrequency=""0.05"" type=""fractalNoise"" numOctaves=""1"" seed=""2"" result=""BOTTOM-SPLASH_10""></feTurbulence>
        <feGaussianBlur stdDeviation=""6.5"" in=""SourceAlpha"" result=""BOTTOM-SPLASH_20""></feGaussianBlur>
        <feDisplacementMap scale=""420"" in=""BOTTOM-SPLASH_20"" in2=""BOTTOM-SPLASH_10"" result=""BOTTOM-SPLASH_30""></feDisplacementMap>
        <feComposite operator=""in"" in=""COLOR-blue"" in2=""BOTTOM-SPLASH_30"" result=""BOTTOM-SPLASH_40""></feComposite>
      <!-- BOTTOM END -->

      <!-- MIDDLE SPLASH -->
        <feTurbulence baseFrequency=""0.1"" type=""fractalNoise"" numOctaves=""1"" seed=""1"" result=""MIDDLE-SPLASH_10""></feTurbulence>
        <feGaussianBlur in=""SourceAlpha"" stdDeviation=""0.1"" result=""MIDDLE-SPLASH_20""></feGaussianBlur>
        <feDisplacementMap in=""MIDDLE-SPLASH_20"" in2=""MIDDLE-SPLASH_10"" scale=""25"" result=""MIDDLE-SPLASH_30""></feDisplacementMap>
        <feComposite in=""COLOR-violet-light"" in2=""MIDDLE-SPLASH_30"" operator=""in"" result=""MIDDLE-SPLASH_40""></feComposite>
      <!-- MIDDLE END -->

      <!-- TOP SPLASH -->
        <feTurbulence baseFrequency=""0.07"" type=""fractalNoise"" numOctaves=""1"" seed=""1"" result=""TOP-SPLASH_10""></feTurbulence>
        <feGaussianBlur stdDeviation=""3.5"" in=""SourceAlpha"" result=""TOP-SPLASH_20""></feGaussianBlur>
        <feDisplacementMap scale=""220"" in=""TOP-SPLASH_20"" in2=""TOP-SPLASH_10"" result=""TOP-SPLASH_30""></feDisplacementMap>
        <feComposite operator=""in"" in=""COLOR-violet"" in2=""TOP-SPLASH_30"" result=""TOP-SPLASH_40""></feComposite>
      <!-- TOP END -->

      <!-- LIGHT EFFECTS -->
        <feMerge result=""LIGHT-EFFECTS_10"">
          <feMergeNode in=""BOTTOM-SPLASH_40""></feMergeNode>
          <feMergeNode in=""MIDDLE-SPLASH_40""></feMergeNode>
          <feMergeNode in=""TOP-SPLASH_40""></feMergeNode>
        </feMerge>
        <feColorMatrix type=""matrix"" values=""0 0 0 0 0,
        0 0 0 0 0,
        0 0 0 0 0,
        0 0 0 1 0"" in=""LIGHT-EFFECTS_10"" result=""LIGHT-EFFECTS_20""></feColorMatrix>
        <feGaussianBlur stdDeviation=""2"" in=""LIGHT-EFFECTS_20"" result=""LIGHT-EFFECTS_30""></feGaussianBlur>
        <feSpecularLighting surfaceScale=""5"" specularConstant="".75"" specularExponent=""30"" lighting-color=""#white"" in=""LIGHT-EFFECTS_30"" result=""LIGHT-EFFECTS_40"">
          <fePointLight x=""-50"" y=""-100"" z=""400""></fePointLight>
        </feSpecularLighting>
        <feComposite operator=""in"" in=""LIGHT-EFFECTS_40"" in2=""LIGHT-EFFECTS_20"" result=""LIGHT-EFFECTS_50""></feComposite>
        <feComposite operator=""arithmetic"" k1=""0"" k2=""1"" k3=""1"" k4=""0"" in=""LIGHT-EFFECTS_10"" in2=""LIGHT-EFFECTS_50"" result=""LIGHT-EFFECTS_60""></feComposite>
      </filter>
      <!-- LIGHT EFFECTS END -->
    </defs>
      <text class=""filtered"" x=""50"" y=""200"">splash!</text>
  </svg>";

        public void LoadSvg()
        {
            var text = File.ReadAllText(@"C:\Users\cdigg\git\ara3d-dev\ara3d.github.io\ara3d.svg");

            var text2 = File.ReadAllText(@"C:\Users\cdigg\Downloads\SVG spilled Water Effect.html");
            Browser.NavigateToString(text2);

            // TODO: this will be fun.
            // this.Browser.CoreWebView2.ExecuteScriptAsync("");

            // TODO: so will this
            //this.Browser.CoreWebView2.PrintToPdfAsync()

            // TODO: this ight be useful
            //this.Browser.CoreWebView2.AddHostObjectToScript("name", null);
        }
    }
}