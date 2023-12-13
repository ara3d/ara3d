using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace KiwiMarkdownEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var html = @"<html><head></head><body>HELLO!</body></html>";
            WebBrowser.NavigateToString(html);
            this.Editor.TextChanged += Editor_TextChanged;
        }

        private void Editor_TextChanged(object? sender, EventArgs e)
        {
            var html = Markdig.Markdown.ToHtml(this.Editor.Text);
            this.WebBrowser.NavigateToString(html);
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var html = Markdig.Markdown.ToHtml(@"<html><head></head><body>HELLO!</body></html>");
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var doc = WebBrowser.Document;
            /*
            var head = doc.GetElementsByTagName("head")[0];
            var script = doc.CreateElement("script");
            script.SetAttribute("text", "function sayHello() { alert('hello'); }");
            head.AppendChild(script);
            doc.InvokeScript("sayHello");
            */
        }
    }
}