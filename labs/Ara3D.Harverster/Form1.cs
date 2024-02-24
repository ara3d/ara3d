using GH_IO.Serialization;

namespace Ara3D.Harverster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var a = new GH_Archive();
            a.ReadFromFile(@"C:\Users\cdigg\dev\grasshopper\first.gh");
            a.WriteToFile(@"C:\Users\cdigg\dev\grasshopper\first-output.ghx", false, false);
        }
    }
}
