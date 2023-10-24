using SvgEditorWinForms.Models;

namespace SvgDemoWinForms
{
    public class DataModelController
    {
        private DocumentModel _documentModel = new();

        public DocumentModel Model
        {
            get => _documentModel;
            set
            {
                _documentModel = value;
                NotifyOfChange();
            }
        }

        public void NotifyOfChange()
            => ModelChanged?.Invoke(this, EventArgs.Empty);

        public event EventHandler? ModelChanged;
    }
}