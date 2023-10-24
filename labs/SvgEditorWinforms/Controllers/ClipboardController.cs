using SvgEditorWinForms.Models;

namespace SvgEditorWinForms
{
    public class ClipboardController
    {
        public event EventHandler ClipboardChanged;

        public List<ElementModel> Elements { get; set; } = new();

        public void SetClipboard(IEnumerable<ElementModel> models)
        {
            Elements = models.ToList();
        }
    }
}