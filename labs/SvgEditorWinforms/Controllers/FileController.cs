
namespace SvgDemoWinForms
{
    public class FileController
    {
        public string CurrentFileName = "";

        private bool _modified; 

        public bool Modified
        {
            get => _modified;
            set
            {
                _modified = value;
                ModifiedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public OpenFileDialog OpenFileDialog { get; } = new ();
        public SaveFileDialog SaveFileDialog { get; } = new ();

        public Action<string> onSave { get; }
        public Action<string> onOpen { get; }

        public string Ext { get; }

        public event EventHandler ModifiedChanged;

        public FileController(string fileExtension, Action<string> onSave, Action<string> onOpen)
        {
            Ext = fileExtension;
            OpenFileDialog.Filter = $"{Ext.ToUpper()} files (*.{Ext}))|*.{Ext}|All files (*.*)|*.*";
            SaveFileDialog.Filter = $"{Ext.ToUpper()} files (*.{Ext}))|*.{Ext}|All files (*.*)|*.*";
            this.onSave = onSave;
            this.onOpen = onOpen;
        }

        public bool SaveWithDialog()
        {
            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
                return false;
            CurrentFileName = SaveFileDialog.FileName;
            onSave(CurrentFileName);
            Modified = false;
            return true;
        }

        public bool SaveWithCurrentName()
        {
            if (string.IsNullOrEmpty(CurrentFileName))
            {
                return SaveWithDialog();
            }

            onSave(CurrentFileName);
            Modified = false;
            return true;
        }

        public static bool YesNo(string msg)
        {
            return MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo)
                   == DialogResult.Yes;
        }

        public bool CheckModifiedAndCanContinue()
        {
            if (!Modified)
                return true;
            if (!YesNo("Document modified, do you want to save?"))
                return true;
            return SaveWithCurrentName();
        }

        public bool NewWithCheck()
        {
            if (!CheckModifiedAndCanContinue()) 
                return false;
            Modified = false;
            CurrentFileName = "";
            return true;
        }

        public bool Open()
        {
            if (!CheckModifiedAndCanContinue())
                return false;

            if (OpenFileDialog.ShowDialog() != DialogResult.OK)
                return false;

            CurrentFileName = OpenFileDialog.FileName;
            onOpen(CurrentFileName);
            Modified = false;
            return true;
        }
    }
}
