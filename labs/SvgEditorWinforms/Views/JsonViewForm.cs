using SvgEditorWinForms.Models;

namespace SvgDemoWinForms
{
    public partial class JsonViewForm : Form
    {
        public JsonViewForm()
            => InitializeComponent();
        
        public void Update(DocumentModel doc)
            => richTextBox1.Text = doc.ToJson();

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
