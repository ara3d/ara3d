using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            this.Editor.TextChanged += Editor_TextChanged;
        }

        private void Editor_TextChanged(object? sender, EventArgs e)
        {
            var obj = this.WebBrowser.Document;
            var html = Markdig.Markdown.ToHtml(this.Editor.Text);
            this.WebBrowser.NavigateToString(html);
        }
    }
}