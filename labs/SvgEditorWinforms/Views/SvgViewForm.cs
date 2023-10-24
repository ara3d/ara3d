using Svg;
using SvgEditorWinForms.Models;

namespace SvgDemoWinForms
{
    public partial class SvgViewForm : Form
    {
        public SvgViewForm()
            => InitializeComponent();

        public void Update(DocumentModel documentModel)
            => Update(documentModel.ToSvg());

        public void Update(SvgDocument svgDocument)
            => Update(svgDocument.GetXML());

        public void Update(string text)
            => richTextBox1.Text = text;

    }
}
