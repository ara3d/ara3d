namespace SvgDemoWinForms
{
    public partial class LogViewForm : Form
    {
        public List<string> Lines { get; } = new();

        public LogViewForm()
        {
            InitializeComponent();
        }

        public void Add(string message)
        {
            Lines.Add(message);
            Update();
        }

        public void Update()
        {
            richTextBox1.Lines = Lines.ToArray();
        }

        public void Clear()
        {
            Lines.Clear();
            Update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }

}
