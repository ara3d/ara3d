using System;
using System.Windows.Forms;
using Ara3D.Logging;
using Ara3D.Parsing.Markdown;

namespace Ara3D.MarkdownEditor
{
    public partial class MarkdownEditor : Form
    {
        public ILogger Logger;
        public RichTextBox RichTextBoxParserErrors;
        public RichTextBox RichTextBoxParserNodes;
        public RichTextBox RichTextBoxParserTree;
        public MarkdownBlockParser BlockParser;

        public MarkdownEditor()
        {
            InitializeComponent();
            Logger = Logger.Create("Log");
            RichTextBoxParserErrors = CreateRichTextBoxPage("Errors");
            RichTextBoxParserNodes = CreateRichTextBoxPage("Nodes");
            RichTextBoxParserTree = CreateRichTextBoxPage("Tree");
            richTextBoxMarkdown.TextChanged += RichTextBoxMarkdownOnTextChanged;
        }

        public RichTextBox CreateRichTextBoxPage(string name)
        {
            tabControl1.TabPages.Add(name);
            var page = tabControl1.TabPages[tabControl1.TabCount - 1];
            var r = new RichTextBox();
            page.Controls.Add(r);
            r.WordWrap = true;
            r.DetectUrls = false;
            r.Dock = DockStyle.Fill;
            r.ReadOnly = true;
            return r;
        }

        private void RichTextBoxMarkdownOnTextChanged(object sender, EventArgs e)
        {
            BlockParser = new MarkdownBlockParser(richTextBoxMarkdown.Text);
            if (RichTextBoxParserNodes != null) RichTextBoxParserNodes.Text = BlockParser.Parser.ParserNodesString;
            if (RichTextBoxParserErrors != null) RichTextBoxParserErrors.Text = BlockParser.Parser.ParserErrorsString;
            if (RichTextBoxParserNodes != null) RichTextBoxParserTree.Text = BlockParser.Parser.ParseXml;
        }
    }
}
